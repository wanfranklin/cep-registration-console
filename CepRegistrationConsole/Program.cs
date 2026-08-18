using CepRegistrationConsole;

var client = new HttpClient();
var menu = new Menu(client);
menu.Run();
