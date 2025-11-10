using AutomatizacionPOM.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using SeleniumExtras.WaitHelpers;

namespace AutomatizacionPOM.Pages
{
    public class FinanciamientoPage
    {
        private IWebDriver driver;
        Utilities utilities;
        private WebDriverWait wait;

        public FinanciamientoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // --- LOCALIZADORES ---
        private By campoCuotas = By.Id("cuota");
        private By campoInicial = By.Id("inicial");
        private By botonGenerar = By.XPath("//span[@class='glyphicon glyphicon-refresh']");
        private By campoDiaPagoCalendario = By.XPath("//input[contains(@class, 'datepicker')]");

        // --- ¡¡LOCALIZADOR CORREGIDO!! ---
        // Es probable que no sea un <a>, sino un <button>.
        // O que el <a> no sea hijo directo.
        // Este XPath busca en CUALQUIER LUGAR del footer un botón o link
        // que diga "ACEPTAR". Es mucho más robusto.
        private By botonAceptarModal = By.XPath("//div[@class='modal-footer']//*[normalize-space()='ACEPTAR']");


        // --- METODOS ---

        public void IngresarCuotas(string numeroCuotas)
        {
            utilities.ClearAndEnterText(campoCuotas, numeroCuotas);
        }

        public void IngresarInicial(string monto)
        {
            utilities.EnterText(campoInicial, monto);
        }

        /// <summary>
        /// ¡NUEVO MÉTODO! Para tu flujo manual.
        /// </summary>
        public void IngresarFecha(string fecha)
        {
            // Escribimos la fecha "04/11/2025" directamente
            utilities.ClearAndEnterText(campoDiaPagoCalendario, fecha);
            // Presionamos Enter para que el calendario se cierre
            utilities.Enter(campoDiaPagoCalendario);
        }

        // (El método SeleccionarDia se queda por si lo usas en otras pruebas)
        public void SeleccionarDia(string dia)
        {
            // Usamos ClearAndEnterText (que ya tiene esperas) para escribir el día.
            utilities.ClearAndEnterText(campoDiaPagoCalendario, dia);

            // Hacemos Enter para que la página (Angular/JS) reconozca el cambio.
            utilities.Enter(campoDiaPagoCalendario);
        }

        // (El método SeleccionarFechaDeHoy se queda por si lo usas en otras pruebas)
        public void SeleccionarFechaDeHoy()
        {
            string fechaDeHoy = DateTime.Now.ToString("dd/MM/yyyy");
            utilities.ClearAndEnterText(campoDiaPagoCalendario, fechaDeHoy);
            utilities.Enter(campoDiaPagoCalendario);
        }

        public void ClicAceptarModal()
        {
            // Ahora usa el localizador corregido
            utilities.ClickButton(botonAceptarModal);
        }

        public void ClicGenerarCronograma()
        {
            utilities.ClickButton(botonGenerar);
        }

        public void VerificarErrorFinanciamiento(string mensaje)
        {
            By textoErrorDinamico = By.XPath($"//*[contains(text(), \"{mensaje}\")]");
            try
            {
                IWebElement errorElement = utilities.WaitForElementToBeVisible(textoErrorDinamico);
                Assert.IsTrue(errorElement.Displayed, "El mensaje de error no está visible.");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"No se encontró el mensaje de error de financiamiento (Timeout): '{mensaje}'");
            }
        }
    }
}