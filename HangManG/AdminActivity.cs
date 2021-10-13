using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HangManG.Common;
using HangManG.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangManG
{
    [Activity(Label = "AdminActivity")]
    public class AdminActivity : Activity
    {
        Button btnSave, btnExit;
        EditText etWord;
        HangmanConnection connection;
        ListView list;
        List<string> words;
        ArrayAdapter<string> adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_admin);
            connection = new HangmanConnection(this);
            words = connection.GetAllWordsString();

            btnExit = FindViewById<Button>(Resource.Id.btnExit);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            etWord = FindViewById<EditText>(Resource.Id.etWord);
            list = FindViewById<ListView>(Resource.Id.list);

            btnExit.Click += BtnExit_Click;
            btnSave.Click += BtnSave_Click;

            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, words);
            list.Adapter = adapter;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string wordstring = etWord.Text.Trim().ToUpper();
            string message = "";
            if (wordstring.Length == 0)
            {
                message = "Please Fill All Boxes";
            }
            else
            {
                Word word = new Word();
                word.Text = wordstring;
                if (connection.SaveWord(word))
                {
                    message = "Word is Saved";
                    words.Add(wordstring);
                    adapter.NotifyDataSetChanged();
                }
                else
                {
                    message = "This Word is Already in List";
                }
            }
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}