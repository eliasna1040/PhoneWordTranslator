using System.Text;

namespace PhoneNumberTranslator;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private string _translatedPhoneNumber = string.Empty;

    private static readonly string[] digits =
    [
        "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ"
    ];

    private void BtnTranslate_OnClicked(object? sender, EventArgs e)
    {
        string phoneword = Phoneword.Text.ToUpper();

        StringBuilder number = new();
        foreach (char character in phoneword)
        {
            if (" -0123456789".Contains(character))
            {
                number.Append(character);
            }
            else
            {
                number.Append(TranslateToNumber(character));
            }
        }

        if (!string.IsNullOrWhiteSpace(number.ToString()))
        {
            _translatedPhoneNumber = number.ToString();
            BtnCall.Text = $"Call {number}";
            BtnCall.IsEnabled = true;
        }
        else
        {
            BtnCall.Text = "Call";
            BtnCall.IsEnabled = false;
        }
    }

    private static int? TranslateToNumber(char c)
    {
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i].Contains(c))
                return 2 + i;
        }

        return null;
    }

    private async void BtnCall_OnClicked(object? sender, EventArgs e)
    {
        if (await this.DisplayAlert(
                "Dial a Number",
                "Would you like to call " + _translatedPhoneNumber + "?",
                "Yes",
                "No"))
        {
            // TODO: dial the phone
            try
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    PhoneDialer.Default.Open(_translatedPhoneNumber);
                }
            }
            catch (ArgumentNullException)
            {
                await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
            }
        }
    }
}