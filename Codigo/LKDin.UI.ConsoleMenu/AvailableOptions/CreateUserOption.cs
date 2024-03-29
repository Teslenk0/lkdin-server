﻿using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class CreateUserOption : UserProtectedConsoleMenuOption
    {
        private readonly IUserLogic _userService;

        public CreateUserOption(string messageToPrint, IUserLogic userService) : base(messageToPrint)
        {
            this._userService = userService;
        }

        protected override async Task PerformExecution()
        {
            UserDTO userDTO = new()
            {
                Id = this.RequestUserId(),
                Name = this.RequestName(),
                Password = this.RequestPassword(),
            };

            await this._userService.CreateUser(userDTO);

            this.PrintFinishedExecutionMessage("Se creo el usuario exitosamente");
        }

        private string RequestName()
        {
            Console.Write("Nombre: ");

            string name;

            do
            {
                name = this.CancelableReadLine();

                if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || !name.All(c => char.IsWhiteSpace(c) || char.IsLetter(c)))
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Nombre: ");
                }
            }
            while (string.IsNullOrWhiteSpace(name) || name.Length < 2 || !name.All(c => char.IsWhiteSpace(c) || char.IsLetter(c)));

            return name;
        }
    }
}
