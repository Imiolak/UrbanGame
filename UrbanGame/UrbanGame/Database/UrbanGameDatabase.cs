using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UrbanGame.Database.Models;
using UrbanGame.Exceptions;
using Xamarin.Forms;

namespace UrbanGame.Database
{
    public class UrbanGameDatabase
    {
        private const string DbFileName = "UrbanGameDb.db3";

        private readonly SQLiteConnection _database;

        public UrbanGameDatabase()
        {
            _database = DependencyService.Get<IDbHelper>().GetConnection(DbFileName);
            CreateTables();
        }

        #region Objectives
        public List<Objective> GetAllObjectives()
        {
            return _database.GetAllWithChildren<Objective>(recursive: true);
        }

        public Objective GetObjectiveByObjectiveNo(int objectiveNo)
        {
            return _database.GetAllWithChildren<Objective>(o => o.ObjectiveNo == objectiveNo, recursive: true)
                .Single();
        }

        public void InsertEmptyObjectives(int numberOfObjectives)
        {
            for (var objecttiveNo = 1; objecttiveNo <= numberOfObjectives; objecttiveNo++)
            {
                var objective = new Objective
                {
                    ObjectiveNo = objecttiveNo
                };

                AddObjective(objective);
            }
        }

        public void AddObjective(Objective objective)
        {
            var numberOfObjectives = int.Parse(GetApplicationVariableByName(ApplicationVariables.NumberOfObjectives).Value);

            if (objective.ObjectiveNo > numberOfObjectives)
            {
                throw new ObjectiveOutOfBoundsException();
            }

            _database.InsertWithChildren(objective, recursive: true);
        }

        public void UpdateObjective(Objective objective)
        {
            _database.UpdateWithChildren(objective);
        }
        #endregion

        #region Objective Steps
        public void AddObjectiveStep(ObjectiveStep objectiveStep)
        {
            var objectiveStepType = Type.GetType(objectiveStep.ObjectiveStepType);
            _database.Insert(objectiveStep, objectiveStepType);
        }

        public void UpdateObjectiveStep(ObjectiveStep objectiveStep)
        {
            _database.Update(objectiveStep, Type.GetType(objectiveStep.ObjectiveStepType));
        }

        public IEnumerable<ObjectiveStep> GetObjectiveStepsByObjectiveId(int objectiveId)
        {
            var objectiveSteps = new List<ObjectiveStep>();

            objectiveSteps.AddRange(_database.Table<GoToLocationObjectiveStep>().Where(s => s.ObjectiveId == objectiveId));
            objectiveSteps.AddRange(_database.Table<QuestionObjectiveStep>().Where(s => s.ObjectiveId == objectiveId));
            objectiveSteps.AddRange(_database.Table<TextObjectiveStep>().Where(s => s.ObjectiveId == objectiveId));
            objectiveSteps.AddRange(_database.Table<EndGameObjectiveStep>().Where(s => s.ObjectiveId == objectiveId));

            return objectiveSteps;
        }

        public ObjectiveStep GetActiveObjectiveStep()
        {
            var allObjectives = GetAllObjectives()
                .OrderBy(obj => obj.ObjectiveNo);

            if (allObjectives.All(obj => obj.IsCompleted))
            {
                return _database.Table<EndGameObjectiveStep>()
                    .Single();
            }

            var activeObjctive = allObjectives
                .FirstOrDefault(obj => obj.IsStarted && !obj.IsCompleted);

            var activeObjectiveStep = GetObjectiveStepsByObjectiveId(activeObjctive.Id)
                .OrderBy(s => s.OrderInObjective)
                .FirstOrDefault(step => !step.IsCompleted);

            if (activeObjectiveStep == null)
            {
                activeObjctive.IsCompleted = true;
                UpdateObjective(activeObjctive);
                
                var nextObjective = GetObjectiveByObjectiveNo(activeObjctive.ObjectiveNo + 1);
                if (!nextObjective.IsStarted)
                {
                    activeObjctive.IsStarted = true;
                    UpdateObjective(nextObjective);
                }

                activeObjectiveStep = GetObjectiveStepsByObjectiveId(nextObjective.Id)
                    .OrderBy(step => step.OrderInObjective)
                    .First();
            }

            activeObjectiveStep.IsStarted = true;
            UpdateObjectiveStep(activeObjectiveStep);

            return activeObjectiveStep;
        }
        
        public void CompleteObjectiveStep(ObjectiveStep objectiveStep)
        {
            objectiveStep.IsCompleted = true;
            UpdateObjectiveStep(objectiveStep);
        }

        public void StartNextObjectiveStep()
        {
            var notCompletedObjectives = _database.Table<Objective>()
                .Where(obj => !obj.IsCompleted)
                .OrderBy(obj => obj.ObjectiveNo);
                
            var firstNotCompletedObjective = notCompletedObjectives.First();
            var firstNotStartedObjectiveStep = GetObjectiveStepsByObjectiveId(firstNotCompletedObjective.Id)
                .FirstOrDefault(step => !step.IsStarted);

            if (firstNotStartedObjectiveStep != null)
            {
                firstNotStartedObjectiveStep.IsStarted = true;
                UpdateObjectiveStep(firstNotStartedObjectiveStep);
            }
        }
        #endregion

        #region Appliation Varibles
        public ApplicationVariable GetApplicationVariableByName(ApplicationVariables name)
        {
            var variableNameString = name.ToString();

            return _database.Find<ApplicationVariable>(v => v.Name == variableNameString);
        }
        
        public void SetApplicationVariable(ApplicationVariables name, string value)
        {
            var variableNameString = name.ToString();
            var variable = _database.Find<ApplicationVariable>(v => v.Name == variableNameString);

            if (variable != null)
            {
                variable.Value = value;
                _database.Update(variable);
            }
            else
            {
                _database.Insert(new ApplicationVariable { Name = name.ToString(), Value = value });
            }
        }
        #endregion

        #region DB Helpers
        private void CreateTables()
        {
            _database.CreateTable<ApplicationVariable>();
            _database.CreateTable<Objective>();
            _database.CreateTable<ObjectiveStep>();
            _database.CreateTable<EndGameObjectiveStep>();
            _database.CreateTable<GoToLocationObjectiveStep>();
            _database.CreateTable<TextObjectiveStep>();
            _database.CreateTable<QuestionObjectiveStep>();
        }

        public void ClearDb()
        {
            _database.DeleteAll<ApplicationVariable>();
            _database.DeleteAll<QuestionObjectiveStep>();
            _database.DeleteAll<GoToLocationObjectiveStep>();
            _database.DeleteAll<EndGameObjectiveStep>();
            _database.DeleteAll<TextObjectiveStep>();
            _database.DeleteAll<ObjectiveStep>();
            _database.DeleteAll<Objective>();
        }
        #endregion
    }
}
