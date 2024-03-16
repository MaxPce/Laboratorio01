using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio01
{
    using System;

    class CuentaBancaria
    {
        public int NumeroCuenta { get; }
        public string TitularCuenta { get; }
        protected decimal Saldo { get; set; }
        protected int PIN { get; set; }

        public CuentaBancaria(int numeroCuenta, string titularCuenta, decimal saldoInicial, int pin)
        {
            NumeroCuenta = numeroCuenta;
            TitularCuenta = titularCuenta;
            Saldo = saldoInicial;
            PIN = pin;
        }

        public decimal ConsultarSaldo(int pin)
        {
            VerificarPIN(pin);
            return Saldo;
        }

        public void Depositar(decimal cantidad)
        {
            Saldo += cantidad;
            Console.WriteLine($"Se depositaron {cantidad:C}. Nuevo saldo: {Saldo:C}");
        }

        public void Retirar(decimal cantidad, int pin)
        {
            VerificarPIN(pin);

            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a retirar debe ser mayor que cero.");
            }

            if (cantidad > Saldo)
            {
                throw new InvalidOperationException("Saldo insuficiente.");
            }

            // Aquí puedes añadir lógica adicional para verificar límites de retiro diario, por ejemplo.

            Saldo -= cantidad;
            Console.WriteLine($"Se retiraron {cantidad:C}. Nuevo saldo: {Saldo:C}");
        }

        public void CambiarPIN(int pinActual, int nuevoPIN)
        {
            VerificarPIN(pinActual);

            if (pinActual == nuevoPIN)
            {
                throw new ArgumentException("El nuevo PIN no puede ser igual al PIN actual.");
            }

            PIN = nuevoPIN;
            Console.WriteLine("PIN cambiado exitosamente.");
        }

        private void VerificarPIN(int pin)
        {
            if (pin != PIN)
            {
                throw new ArgumentException("PIN incorrecto.");
            }
        }
    }

    class CajeroAutomatico : CuentaBancaria
    {
        public CajeroAutomatico(int numeroCuenta, string titularCuenta, decimal saldoInicial, int pin)
            : base(numeroCuenta, titularCuenta, saldoInicial, pin)
        {
        }

        public void RealizarTransaccion(int opcion, decimal cantidad = 0, int nuevoPIN = 0)
        {
            try
            {
                switch (opcion)
                {
                    case 1:
                        Console.WriteLine($"Saldo actual: {ConsultarSaldo(PIN):C}");
                        break;
                    case 2:
                        Depositar(cantidad);
                        break;
                    case 3:
                        Retirar(cantidad, PIN);
                        break;
                    case 4:
                        CambiarPIN(PIN, nuevoPIN);
                        break;
                    default:
                        throw new ArgumentException("Opción de transacción inválida.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Crear una cuenta bancaria
            CajeroAutomatico cajero = new CajeroAutomatico(123456789, "John Doe", 1000, 1234);

            // Interacción con el usuario
            Console.WriteLine("Bienvenido al Cajero Automático");
            int opcion;

            do
            {
                Console.WriteLine("\n1. Consultar Saldo");
                Console.WriteLine("2. Depositar Fondos");
                Console.WriteLine("3. Retirar Efectivo");
                Console.WriteLine("4. Cambiar PIN");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida. Por favor, seleccione un número del menú.");
                    continue;
                }

                if (opcion != 5)
                {
                    switch (opcion)
                    {
                        case 1:
                            cajero.RealizarTransaccion(1);
                            break;
                        case 2:
                            Console.Write("Ingrese la cantidad a depositar: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal cantidadDeposito))
                            {
                                Console.WriteLine("Cantidad inválida.");
                                continue;
                            }
                            cajero.RealizarTransaccion(2, cantidadDeposito);
                            break;
                        case 3:
                            Console.Write("Ingrese la cantidad a retirar: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal cantidadRetiro))
                            {
                                Console.WriteLine("Cantidad inválida.");
                                continue;
                            }
                            cajero.RealizarTransaccion(3, cantidadRetiro);
                            break;
                        case 4:
                            Console.Write("Ingrese el nuevo PIN: ");
                            if (!int.TryParse(Console.ReadLine(), out int nuevoPIN))
                            {
                                Console.WriteLine("PIN inválido.");
                                continue;
                            }
                            cajero.RealizarTransaccion(4, 0, nuevoPIN);
                            break;
                        default:
                            Console.WriteLine("Opción inválida.");
                            break;
                    }
                }
            } while (opcion != 5);

            Console.WriteLine("Gracias por utilizar el Cajero Automático.");
        }
    }


}
