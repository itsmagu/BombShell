namespace BombShell;

using Godot;

public partial class MainNode : Node
{
    [Export] public LineEdit Shell;
    [Export] public RichTextLabel Log;
    private const uint MAX_HISTORY = 10;
    public override void _Ready(){
        GD.Print("Booted!");
        Log.Text =
            "Welcome Netrunner, a VM has been setup on this machine for your protection!\n" +
            "A connection to your VM is open from this window, Good Diving ;)" +
            "\nThis game is made to explore how computers work and how to manipulate them." +
            "\nIn this window(s) no harm can actually be done since it is all just a game";
        Shell.GrabFocus();
    }
    public override void _PhysicsProcess(double delta){ }
    public override void _Input(InputEvent @event){
        if (@event is InputEventKey { Pressed: true } eventKey)
            switch (eventKey.Keycode){
            case Key.Enter:
                Log.Text += $"\n>{Shell.Text}";
                RunCommand(Shell.Text);
                Shell.Text = "";
                break;
            case Key.Escape:
                Shell.Text = "";
                GetViewport().SetInputAsHandled();
                break;
            case Key.F5:
                Log.Text = "~Reset Console";
                break;
            }
    }
    void Tokenize(string commandString){
        Log.Text += "\n&";
        int[] tokens = new int[5];
        int current = 0;
        while (current < commandString.Length){
            switch (commandString[current]){
            case ' ':
                Log.Text += "+";
                break;
            default:
                Log.Text += "_";
                break;
            }
            current++;
        }
        Log.Text += "\n";
    }
    void RunCommand(string commandString){
        Tokenize(commandString: commandString);
    }
}