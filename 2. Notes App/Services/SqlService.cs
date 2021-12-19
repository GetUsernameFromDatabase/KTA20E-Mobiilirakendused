using NotesApp.Models;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace NotesApp.Services
{
    public class SqlService
    {
        SQLiteConnection _sqlConnection;

        public SqlService()
        {
            CreateConnection();
            _sqlConnection.CreateTable<Note>();
        }

        private void CreateConnection()
        {                                 
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "notesdb.db3");
            _sqlConnection = new SQLiteConnection(dbPath);
        }        

        public void InsertData(Note note)
        {
            _sqlConnection.Insert(note);
        }

        public void UpdateData(Note note)
        {
            _sqlConnection.Update(note);
        }

        public List<Note> GetAllNotes()
        {
            return _sqlConnection.Table<Note>().ToList();
        }

        public Note GetNote(int id)
        {
            return _sqlConnection.Table<Note>().Where(x => x.Id == id).FirstOrDefault();
        }

        public void DeleteData(Note note)
        {
            _sqlConnection.Delete(note);
        }
    }
}