namespace NetFrontendDayApp;

public partial class SpeakersPage : ContentPage
{
	public SpeakersPage()
	{
		InitializeComponent();

       Root.Parameters = new Dictionary<string, object>()
       {
           { "Url", "/speakers/2022" }
       };
    }
}
