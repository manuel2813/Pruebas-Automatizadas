using AutomatizacionPOM.Pages.Helpers; // <--- ¡LA LÍNEA QUE ARREGLA EL ERROR!
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutomatizacionPOM.Pages
{
    public class AccessPage
    {
        private IWebDriver driver;
        Utilities utilities; // <-- Esta línea ya no dará error

        public AccessPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver); // <-- Esta línea ya no dará error
        }

        // LOGIN
        private By usernameField = By.XPath("//input[@id='Email']");
        private By passwordField = By.XPath("//input[@id='Password']");
        private By loginButton = By.XPath("//button[normalize-space()='Iniciar']");
        private By acceptButton = By.XPath("//button[contains(text(),'Aceptar')]");
        private By logo = By.XPath("//img[@id='ImagenLogo']");

        // VENTAS
        private By VentaField = By.XPath("//a[@class='menu-lista-cabecera']/span[text()='Venta']");
        private By NuevaVentaField = By.XPath("//a[normalize-space()='Nueva Venta']");

        //RESTAURANTE
        private By RestauranteField = By.XPath("//a[@class='menu-lista-cabecera']/span[text()='Restaurante']");
        private By AtencionField = By.XPath("//a[normalize-space()='Atención']");

        // GASTOS
        private By GastoField = By.XPath("//span[normalize-space()='Gasto']");
        private By VerGastoField = By.XPath("//a[@href='/Gasto/Index']");
        private By ConceptoField = By.XPath("//a[normalize-space()='Concepto']");
        private By GastoReporteField = By.XPath("//a[normalize-space()='Reporte']"); // <--- AÑADIDO

        // TESORERÍA (NUEVO MÓDULO)
        private By TesoreriaField = By.XPath("//span[normalize-space()='Tesorería y Finanzas']");
        private By CuentasPorPagarField = By.XPath("//a[normalize-space()='Cuentas por Cobrar/Pagar']");
        private By IngresosEgresosField = By.XPath("//a[normalize-space()='Ingresos/Egresos']");


        public void OpenToAplicattion(string url)
        {
            driver.Navigate().GoToUrl(url);
            // Ya no usamos Thread.Sleep, tu nuevo Utilities.cs es mejor
        }

        public void LoginToApplication(string _username, string _password)
        {
            // (Este es tu método de login original, lo respetamos)
            utilities.EnterText(usernameField, _username);
            //Thread.Sleep(2000); // Ya no es necesario con el nuevo Utilities

            utilities.EnterText(passwordField, _password);
            //Thread.Sleep(2000); // Ya no es necesario con el nuevo Utilities

            utilities.ClickButton(loginButton);
            //Thread.Sleep(4000); // Ya no es necesario con el nuevo Utilities

            utilities.ClickButton(acceptButton);
            //Thread.Sleep(4000); // Ya no es necesario con el nuevo Utilities

            // Comprobar que el login fue exitoso
            // Usamos el helper de espera para más estabilidad
            utilities.WaitForElementToBeVisible(logo);
            var succesElement = driver.FindElement(logo);
            Assert.IsNotNull(succesElement, "No se encontró el elemento de éxito después del login.");
        }

        public void enterModulo(string _modulo)
        {
            switch (_modulo)
            {
                case "Venta":
                    utilities.ClickButton(VentaField);
                    break;
                case "Restaurante":
                    utilities.ClickButton(RestauranteField);
                    break;
                case "Gasto":
                    utilities.ClickButton(GastoField);
                    break;

                // --- ¡NUEVO CASE AÑADIDO! ---
                case "Tesorería y Finanzas":
                    utilities.ClickButton(TesoreriaField);
                    break;

                default:
                    throw new ArgumentException($"El {_modulo} no es válido.");
            }
            // Thread.Sleep(4000); // Ya no es necesario con el nuevo Utilities
        }

        public void enterSubModulo(string _submodulo)
        {
            // Esperamos a que el overlay (si existe) desaparezca
            utilities.WaitForBlockOverlayToDisappear();

            switch (_submodulo)
            {
                case "Nueva Venta":
                    utilities.ClickButton(NuevaVentaField);
                    break;
                case "Atención":
                    utilities.ClickButton(AtencionField);
                    break;
                case "Ver Gasto":
                    utilities.ClickButton(VerGastoField);
                    break;
                case "Concepto":
                    utilities.ClickButton(ConceptoField);
                    break;

                // --- ¡NUEVOS CASES AÑADIDOS! ---
                case "Reporte de Gasto":
                    utilities.ClickButton(GastoReporteField);
                    break;
                case "Cuentas Por Cobrar/Pagar":
                    utilities.ClickButton(CuentasPorPagarField);
                    break;
                case "Ingresos/Egresos":
                    utilities.ClickButton(IngresosEgresosField);
                    break;

                default:
                    throw new ArgumentException($"El {_submodulo} no es válido.");
            }
            // Thread.Sleep(10000); // Ya no es necesario con el nuevo Utilities
        }
    }
}