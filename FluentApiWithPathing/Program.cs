using FluentApiWithPathing;

// Notice how you are restricted to send a command as
//      server and command
//      OR
//      server, username, password and command
// You cannot, for instance, specify and username with no password
string commandOutputSimple = ServerCommand
                            .CreateCommand()
                            .ToServerUnauthenticated("Server-01")
                            .SendCommand("ping")
                            .Run();
Console.WriteLine(commandOutputSimple);


string commandOutputFull = ServerCommand
                            .CreateCommand()
                            .ToServerWithAuthentication("Server-01")
                            .WithUsername("User1")
                            .WithPassword("P@ssw0rd1!")
                            .SendCommand("ping")
                            .Run();
Console.WriteLine(commandOutputFull);