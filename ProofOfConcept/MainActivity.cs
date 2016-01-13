using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ProofOfConcept
{
    [Activity(Label = "Phone Number Translater Thing", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it

            EditText inputNumberText = FindViewById<EditText>(Resource.Id.inputNumber);
            Button translateButton = FindViewById<Button>(Resource.Id.translateButton);
            Button callButton = FindViewById<Button>(Resource.Id.callButton);

            // Disable the "Call" button
            callButton.Enabled = false;

            // Add code to translate number
            string translatedNumber = string.Empty;

            translateButton.Click += (object sender, EventArgs e) =>
            {
                // Translate user's alphanumeric phone number to numeric
                translatedNumber = Core.PhoneTranslator.ToNumber(inputNumberText.Text);
                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = "Call " + translatedNumber;
                    callButton.Enabled = true;
                }
            };

            callButton.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    // Create intent to dial phone
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };
    
        }
    }
}

