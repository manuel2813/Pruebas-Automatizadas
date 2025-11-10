using AutomatizacionPOM.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; // <-- Añadido para las esperas
using System;
using System.Linq;

namespace AutomatizacionPOM.Pages
{
    public class RegistroGastoPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public RegistroGastoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // --- LOCALIZADORES ---
        private By tituloModal = By.XPath("//h4[normalize-space()='REGISTRO DE GASTO']");
        private By campoTotal = By.XPath("//input[@ng-model='gasto.Total']");
        private By botonGuardar = By.XPath("//button[normalize-space()='GUARDAR']");
        private By campoFecha = By.Id("fechaRegistro");
        private By celdasError = By.XPath("//*[@style='color:#bb1f1f'][normalize-space()]");
        private By campoImporte = By.XPath("//input[@ng-model='gasto.Importe']");
        private By campoProveedor = By.Id("DocumentoIdentidad");

        // CORREGIDO: Apuntamos al LABEL, no al INPUT (que suele estar oculto)
        private By checkIGV = By.XPath("//label[@for='ventaigv']");

        // --- ¡NUEVOS LOCALIZADORES DE CRÉDITO! ---

        // CORREGIDO: Apuntamos al LABEL, no al INPUT
        private By checkAlCredito = By.XPath("//label[@for='credito']");

        // CORREGIDO: Apuntamos al LABEL, no al INPUT
        private By radioConfigurar = By.XPath("//label[@for='radio2']");

        private By tituloModalFinanciamiento = By.XPath("//h4[normalize-space()='FINANCIAMIENTO']");


        // --- METODOS (TODOS LIMPIOS Y SIN THREAD.SLEEP) ---

        public void VerificarPaginaRegistroGasto()
        {
            // Reemplazamos Thread.Sleep con una espera explícita
            var titulo = utilities.WaitForElementToBeVisible(tituloModal);
            Assert.IsTrue(titulo.Displayed,
                "No se navegó al formulario de Registro de Gasto (Título no encontrado).");
        }

        public void IngresarTotal(string importe)
        {
            // Usamos el EnterText limpio
            utilities.EnterText(campoTotal, importe);
            // Hacemos clic en el título para quitar el foco (blur) y activar cálculos
            utilities.ClickButton(tituloModal);
        }

        public void HacerClickGuardar()
        {
            utilities.ClickButton(botonGuardar);
        }

        public string ObtenerMensajeInconsistencia()
        {
            // Esperamos a que la primera celda de error sea visible
            utilities.WaitForElementToBeVisible(celdasError);
            var elementosError = driver.FindElements(celdasError);

            if (elementosError.Count == 0)
            {
                Assert.Fail("No se encontró ningún mensaje de error de inconsistencia.");
            }
            string todosLosErrores = string.Join(" ", elementosError.Select(e => e.Text));
            return todosLosErrores;
        }

        public void IngresarFecha(string fecha)
        {
            var elementoFecha = utilities.WaitForElementToBeVisible(campoFecha);
            elementoFecha.Clear();

            if (!string.IsNullOrEmpty(fecha))
            {
                elementoFecha.SendKeys(fecha);
            }
            utilities.ClickButton(tituloModal); // Quitar foco
        }

        public string ObtenerValorTotal()
        {
            // Esperamos a que el overlay (cálculo de Angular) desaparezca
            utilities.WaitForBlockOverlayToDisappear();
            // Esperamos al campo y obtenemos el valor
            return utilities.WaitForElementToBeVisible(campoTotal).GetAttribute("value");
        }

        public void IngresarImporte(string importe)
        {
            utilities.EnterText(campoImporte, importe);
        }

        public void BuscarYSeleccionarProveedor(string proveedor)
        {
            utilities.EnterText(campoProveedor, proveedor);
            // Esperamos a que aparezcan las opciones (overlay de búsqueda)
            utilities.WaitForBlockOverlayToDisappear();
            utilities.Enter(campoProveedor);
        }

        public void MarcarCasilla(string nombreCasilla)
        {
            By locator = null;
            if (nombreCasilla == "GASTO CON IGV")
            {
                locator = checkIGV; // Usa el label
            }
            else if (nombreCasilla == "AL CRÉDITO")
            {
                locator = checkAlCredito; // Usa el label
            }
            else
            {
                Assert.Fail($"La casilla '{nombreCasilla}' no está implementada.");
            }

            utilities.ClickButton(locator);

            // Esperamos a que Angular termine de recalcular (si lo hace)
            utilities.WaitForBlockOverlayToDisappear();
        }

        // --- ¡NUEVOS MÉTODOS AÑADIDOS! ---
        public void SeleccionarOpcionConfigurar()
        {
            utilities.ClickButton(radioConfigurar); // Usa el label
        }

        public void VerificarModalFinanciamiento()
        {
            // Esperamos a que el título del NUEVO modal aparezca
            var titulo = utilities.WaitForElementToBeVisible(tituloModalFinanciamiento);
            Assert.IsTrue(titulo.Displayed, "El modal de FINANCIAMIENTO no apareció.");
        }
    }
}