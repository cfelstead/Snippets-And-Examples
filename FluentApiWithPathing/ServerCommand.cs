using System.Text;
using static FluentApiWithPathing.ServerCommand;

namespace FluentApiWithPathing;
internal sealed class ServerCommand :
    // These interfaces are below. They are the different stages in the Fluent APIs flow.
    ISetServerAddress,
    ISetUsername,
    ISetPassword,
    ISetCommand,
    ISend
{
    // These are the variables that will be populated through the Fluent APIs use
    private string _server;
    private string _username;
    private string _password;
    private string _command;



#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // This is set to private in order to force the user to use the Fluent API
    private ServerCommand() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



    // The entry point. The interface is the first step in the process with the initial method you want the user to call
    public static ISetServerAddress CreateCommand() => new ServerCommand();

    // A standard set. You have the method name with any properties. The interface returned is the next setup in the chain.
    public ISetUsername ToServerWithAuthentication(string server)
    {
        _server = server;
        return this;
    }

    // This mirrors the setup above as it is a fork in the path and allows the user to skip some of the sections.
    public ISetCommand ToServerUnauthenticated(string server)
    {
        _server = server;
        return this;
    }

    public ISetPassword WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public ISetCommand WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public ISend SendCommand(string command)
    {
        _command = command;
        return this;
    }

    // The final output
    public string Run()
    {
        StringBuilder sb = new();
        sb.Append($"Running command '{_command}' on the server '{_server}'");

        if (string.IsNullOrWhiteSpace(_username) == false)
        {
            sb.Append($" with the username and password of '{_username}' and '{_password}");
        }

        sb.Append(".");
        return sb.ToString();
    }



    public interface ISetServerAddress
    {
        ISetUsername ToServerWithAuthentication(string server);
        ISetCommand ToServerUnauthenticated(string server);
    }

    public interface ISetUsername
    {
        ISetPassword WithUsername(string username);
    }

    public interface ISetPassword
    {
        ISetCommand WithPassword(string password);
    }

    public interface ISetCommand
    {
        ISend SendCommand(string command);
    }

    public interface ISend
    {
        string Run();
    }
}
