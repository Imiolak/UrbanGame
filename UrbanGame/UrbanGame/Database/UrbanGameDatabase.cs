using SQLite.Net;
using SQLiteNetExtensions.Extensions;
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

        public Objective GetObjectiveForClue(int clueMajor)
        {
            return _database.GetAllWithChildren<Objective>(o => o.ObjectiveNo == clueMajor, recursive: true)
                .Single();
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

        #region Clues
        public void AddClue(Clue clue)
        {
            _database.Insert(clue);
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
            _database.CreateTable<Clue>();
        }

        public void ClearDb()
        {
            _database.DeleteAll<ApplicationVariable>();
            _database.DeleteAll<Clue>();
            _database.DeleteAll<Objective>();
        }
        #endregion
    }
}
