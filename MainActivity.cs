using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Linq;

namespace FirstApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText[] calcNumbers = new EditText[2];
        TextView calculationResult;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            this.calcNumbers[0] = FindViewById<EditText>(Resource.Id.number1);
            this.calcNumbers[1] = FindViewById<EditText>(Resource.Id.number2);
            this.calculationResult = FindViewById<TextView>(Resource.Id.calcResult);

            addEventsToButtons();

        }
        private void addEventsToButtons()
        {
            Button addButton = FindViewById<Button>(Resource.Id.add_Button);
            Button subButton = FindViewById<Button>(Resource.Id.subtract_Button);
            Button mltButton = FindViewById<Button>(Resource.Id.multiply_Button);
            Button divButton = FindViewById<Button>(Resource.Id.divide_Button);

            addButton.Click += (obj, e) =>
            {
                calculationResult.Text = getCalculatorResult(getCalculatorInput(), new[] { '+' });
            };
            subButton.Click += (obj, e) =>
            {
                calculationResult.Text = getCalculatorResult(getCalculatorInput(), new[] { '-' });
            };
            mltButton.Click += (obj, e) =>
            {
                calculationResult.Text = getCalculatorResult(getCalculatorInput(), new[] { '*' });
            };
            divButton.Click += (obj, e) =>
            {
                calculationResult.Text = getCalculatorResult(getCalculatorInput(), new[] { '/' });
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private double[] getCalculatorInput()
        {
            var calcNumberLength = calcNumbers.Length;
            var output = new double[calcNumberLength];

            for (var i = 0; i < calcNumberLength; i++)
                output[i] = double.Parse(calcNumbers[i].Text);

            return output;
        }

        private string getCalculatorResult(double[] numbers, char[] operations)
        {
            var result = numbers[0];
            string resultString = result.ToString();

            for (int i = 1; i < numbers.Length; i++)
            {
                var number = numbers[i];
                char op = operations[i - 1];

                switch (op)
                {
                    case '+':
                        result += number;
                        break;
                    case '-':
                        result -= number;
                        break;
                    case '*':
                        result *= number;
                        break;
                    case '/':
                        result /= number;
                        break;
                    default:
                        throw new System.ArgumentException("The following operator is invaid: " + op);
                }
                resultString += string.Format(" {0} {1}", op, number);
            }

            resultString += " = " + result.ToString();
            return resultString;
        }
    }
}