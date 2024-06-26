using CommunityToolkit.Maui.Views;
using SeriesTracker.Services.Constant;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.Controls.CustomPopUp;

public partial class ActivePagePopUp : Popup
{
    public static readonly BindableProperty TitleProperty =
              BindableProperty.Create(nameof(Title), typeof(string), typeof(ActivePagePopUp), string.Empty);
    public static readonly BindableProperty DetachTextProperty =
          BindableProperty.Create(nameof(DetachText), typeof(string), typeof(ActivePagePopUp), string.Empty);

    public static readonly BindableProperty myContePageProperty =
            BindableProperty.Create(nameof(ContentPageBehavior), typeof(ContentPage), typeof(ActivePagePopUp));

    public static readonly BindableProperty CloseCommandProperty =
        BindableProperty.Create(nameof(CloseCommand), typeof(Command), typeof(ActivePagePopUp));

    public static readonly BindableProperty EditCommandProperty =
        BindableProperty.Create(nameof(EditCommand), typeof(Command), typeof(ActivePagePopUp));

    public static readonly BindableProperty DetachCommandProperty =
        BindableProperty.Create(nameof(DetachCommand), typeof(Command), typeof(ActivePagePopUp));

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(Command), typeof(ActivePagePopUp));

    public ActivePagePopUp()
	{
		InitializeComponent();
        if (WachedFlag == false)       
        DetachText = "Пометить как просмотренное";
        else DetachText = "Пометить как непросмотренное";
    }

    public ContentPage ContentPageBehavior
    {
        get => (ContentPage)GetValue(myContePageProperty);
        set => SetValue(myContePageProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    
    }

    public string DetachText
    {
        get => (string)GetValue(DetachTextProperty);
        set => SetValue(DetachTextProperty, value);

    }

    public Command CloseCommand
    {
        get => (Command)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public Command EditCommand
    {
        get => (Command)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public Command DetachCommand
    {
        get => (Command)GetValue(DetachCommandProperty);
        set => SetValue(DetachCommandProperty, value);
    }

    public Command DeleteCommand
    {
        get => (Command)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        EditCommand?.Execute(null);
        this.Close();
    }

    private void DetachButton_Clicked(object sender, EventArgs e)
    {
        DetachCommand?.Execute(null);
        this.Close();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
        var popup = new DeleteAlert
        {
            Title = Title
        };
        var result = await ContentPageBehavior.ShowPopupAsync(popup);

        if (result is bool boolResult)
        {
            if (boolResult)
            {
                DeleteCommand?.Execute(null);
            }
        }
        return;
    }
}