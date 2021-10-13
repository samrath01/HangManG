using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HangManG.Adapter;
using HangManG.DataLayer;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangManG
{
    [Activity(Label = "HangmanActivity")]
    public class HangmanActivity : Activity
    {
        WordPicker picker;
        HangmanConnection connection;
        TextView word;
        ImageView imageHang;
        GridView buttons;
        ButtonsAdapter adapter;
        Button btnPlay, btnReset, btnBack;
        int currentImage;
        string currentWord;
        string guess;
        string name;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_hangman);
            picker = new WordPicker(this);
            connection = new HangmanConnection(this);
            name = Intent.GetStringExtra("name");

            word = FindViewById<TextView>(Resource.Id.word);
            imageHang = FindViewById<ImageView>(Resource.Id.hang);
            buttons = FindViewById<GridView>(Resource.Id.buttons);
            btnPlay = FindViewById<Button>(Resource.Id.play);
            btnReset = FindViewById<Button>(Resource.Id.reset);
            btnBack = FindViewById<Button>(Resource.Id.back);

            btnPlay.Click += BtnPlay_Click;
            btnReset.Click += BtnReset_Click;
            btnBack.Click += BtnBack_Click;

            Play();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void Play()
        {
            adapter = new ButtonsAdapter(this);
            buttons.Adapter = adapter;

            currentWord = picker.GetGameWord();

            guess = "";
            for (int index = 0; index < currentWord.Length; index++)
            {
                guess += "_";
            }
            word.Text = ConvertToResult();
            btnPlay.Enabled = false;
            btnBack.Enabled = true;
        }

        private string ConvertToResult()
        {
            string result = "";
            for (int index = 0; index < guess.Length; index++)
            {
                result += guess[index] + " ";
            }
            return result;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            DisplayAlert("OOPS!!!", "You Reset The Game So You Lost The Game");
            connection.UpdateProfile(name, false);
            currentImage = 0;
            btnPlay.Enabled = false;
            btnBack.Enabled = true;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        [Export("buttonClicked")]
        public void buttonPressed(View view)
        {
            //user has pressed a letter to guess
            string ltr = ((TextView)view).Text;
            char letter = ltr[0];

            view.Enabled = false;
            view.SetBackgroundColor(Color.White);

            bool right = false;
            string result = "";
            for (int index = 0; index < currentWord.Length; index++)
            {
                if (currentWord[index] == letter)
                {
                    right = true;
                    result += letter;
                }
                else
                {
                    result += guess[index];
                }
            }
            guess = result;
            word.Text = ConvertToResult();
            Toast.MakeText(this, currentWord, ToastLength.Long).Show();
            if (right)
            {
                if (guess.Equals(currentWord))
                {
                    currentImage = 0;
                    DisplayAlert("GAME STATUS", "You Won The Game");
                    connection.UpdateProfile(name, true);
                    word.Text = "";
                    btnPlay.Enabled = true;
                    btnReset.Enabled = false;
                    imageHang.SetImageResource(Resource.Drawable.win);
                }
            }
            else
            {
                currentImage++;
                if (currentImage == 7)
                {
                    currentImage = 0;
                    DisplayAlert("GAME STATUS!!!", "You Lost The Game");
                    connection.UpdateProfile(name, false);
                    word.Text = "";
                    btnPlay.Enabled = true;
                    btnReset.Enabled = false;
                }
                else
                {
                    ChangeImage();
                }

            }
        }

        public void ChangeImage()
        {
            if (currentImage >= 0 && currentImage <= 6)
            {
                int image_id = Resource.Drawable.hang0;
                switch (currentImage)
                {
                    case 1:
                        image_id = Resource.Drawable.hang1;
                        break;
                    case 2:
                        image_id = Resource.Drawable.hang2;
                        break;
                    case 3:
                        image_id = Resource.Drawable.hang3;
                        break;
                    case 4:
                        image_id = Resource.Drawable.hang4;
                        break;
                    case 5:
                        image_id = Resource.Drawable.hang5;
                        break;
                    case 6:
                        image_id = Resource.Drawable.hang6;
                        break;
                    default:
                        image_id = Resource.Drawable.hang0;
                        break;
                }
                imageHang.SetImageResource(image_id);
            }

        }

        private void DisplayAlert(string title, string message)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetNegativeButton("Close", (c, v) =>
            {
                alert.Dispose();
                ChangeImage();
            });
            alert.Show();
        }
    }
}