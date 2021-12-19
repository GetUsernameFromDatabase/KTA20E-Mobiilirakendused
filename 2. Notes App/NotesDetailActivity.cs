using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using NotesApp.Models;
using System;

namespace NotesApp
{
    [Activity(Label = "NotesDetailActivity")]
    public class NotesDetailActivity : Activity
    {
        EditText _headingEditText;
        EditText _contentEditText;
        Note _note;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.notes_detail_layout);

            _headingEditText = FindViewById<EditText>(Resource.Id.headingEditText);
            _contentEditText = FindViewById<EditText>(Resource.Id.contentEditText);
            var saveButton = FindViewById<Button>(Resource.Id.saveButton);
            var deleteButton = FindViewById<Button>(Resource.Id.deleteButton);

            saveButton.Click += SaveButton_Click;
            deleteButton.Click += DeleteButton_Click;
            deleteButton.Visibility = Android.Views.ViewStates.Invisible;

            var mode = Intent.GetStringExtra("mode");
            if(mode == "edit")
            {
                deleteButton.Visibility = Android.Views.ViewStates.Visible;
                var id = Intent.GetIntExtra("id", 0);
                _note = MainActivity.SqlService.GetNote(id);
                _headingEditText.Text = _note.Heading;
                _contentEditText.Text = _note.Content;
            }
            
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            MainActivity.SqlService.DeleteData(_note);
            Finish();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(_note == null)
            {
                var newNote = new Note
                {
                    Heading = _headingEditText.Text,
                    Content = _contentEditText.Text,
                    ChangeDateTime = DateTime.Now
                };
                MainActivity.SqlService.InsertData(newNote);
            }
            else
            {
                _note.Heading = _headingEditText.Text;
                _note.Content = _contentEditText.Text;
                _note.ChangeDateTime = DateTime.Now;
                MainActivity.SqlService.UpdateData(_note);
            }            
            Finish();
        }
    }
}