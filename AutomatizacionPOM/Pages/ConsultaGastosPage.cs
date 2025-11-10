using AutomatizacionPOM.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; // <-- AÑADIR ESTE USING
using System;
using System.Linq;
using System.Threading;
using SeleniumExtras.WaitHelpers; // <-- AÑADIR ESTE USING

namespace AutomatizacionPOM.Pages
{
    public class ConsultaGastosPage
    {
        private IWebDriver driver;
        Utilities utilities;
        private WebDriverWait wait; // <-- AÑADIR WAIT

        public ConsultaGastosPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // <-- AÑADIR WAIT
        }

        // --- LOCALIZADORES ---
        private By botonNuevoGasto = By.XPath("//span[normalize-space()='NUEVO GASTO']");
        private By campoBuscar = By.XPath("//input[@class='form-control input-sm']");
        private By filaVacia = By.XPath("//td[@class='dataTables_empty']");

        // --- Localizadores de Fila 1 ---
        private By botonAnularPrimeraFila = By.XPath("//table[@id='tabla-gastos']/tbody/tr[1]/td[9]/a[2]");
        private By botonVerEditarPrimeraFila = By.XPath("//table[@id='tabla-gastos']/tbody/tr[1]/td[9]/a[1]");
        private By celdaEstadoPrimeraFila = By.XPath("//table[@id='tabla-gastos']/tbody/tr[1]/td[8]");

        // --- Localizadores de Modales ---
        private By tituloModalAnulacion = By.XPath("//h4[normalize-space()='ANULAR GASTO']");
        private By textAreaObservacionAnular = By.XPath("//textarea[contains(@class, 'form-control')]");
        private By botonAceptarAnular = By.XPath("//a[normalize-space()='ACEPTAR']");

        // --- ¡NUEVOS LOCALIZADORES PARA MODAL DETALLE! ---
        private By tituloModalDetalle = By.XPath("//h4[normalize-space()='DETALLE DE GASTO']");
        private By botonCerrarModalDetalle = By.XPath("//button[@title='Cerrar']");


        // --- METODOS ---
        public void HacerClickNuevoGasto()
        {
            utilities.ClickButton(botonNuevoGasto);
        }

        // --- MÉTODOS DE BÚSQUEDA Y GRILLA (CORREGIDOS) ---
        public void BuscarGasto(string texto)
        {
            utilities.ClearAndEnterText(campoBuscar, texto);
            utilities.Enter(campoBuscar); // Presiona Enter para activar la búsqueda

            // Esperamos a que la grilla reaccione (ya sea mostrando datos o fila vacía)
            try
            {
                wait.Until(d => d.FindElement(botonVerEditarPrimeraFila).Displayed ||
                               d.FindElement(filaVacia).Displayed);
            }
            catch (Exception)
            {
                // Ignoramos, VerificarSiHayResultados se encargará
            }
        }

        private void VerificarSiHayResultados()
        {
            utilities.WaitForBlockOverlayToDisappear();

            try
            {
                // Esperamos MÁXIMO 5 segundos a que la primera fila aparezca
                var waitCorto = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                waitCorto.Until(ExpectedConditions.ElementIsVisible(botonVerEditarPrimeraFila));
                // ¡Sí hay resultados!
            }
            catch (WebDriverTimeoutException)
            {
                // No apareció la fila. Verificamos si es porque la tabla está vacía.
                try
                {
                    if (driver.FindElement(filaVacia).Displayed)
                    {
                        Assert.Fail("La búsqueda no arrojó resultados. No se puede continuar.");
                    }
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail("Error inesperado. La tabla no cargó resultados ni el mensaje 'vacío'.");
                }
            }
        }

        public void ClicAnularPrimerGasto()
        {
            VerificarSiHayResultados(); // Comprueba antes de hacer clic
            utilities.ClickButton(botonAnularPrimeraFila);
        }

        public void ClicVerEditarPrimerGasto()
        {
            VerificarSiHayResultados(); // Comprueba antes de hacer clic
            utilities.ClickButton(botonVerEditarPrimeraFila);
        }

        // --- MÉTODOS DE VERIFICACIÓN (CORREGIDOS) ---
        public void VerificarModalAnulacion()
        {
            // Reemplazamos Thread.Sleep por espera explícita
            IWebElement titulo = utilities.WaitForElementToBeVisible(tituloModalAnulacion);
            Assert.IsTrue(titulo.Displayed, "El modal de ANULAR GASTO no apareció.");
        }

        public void VerificarEstadoGasto(string estadoEsperado)
        {
            // Reemplazamos Thread.Sleep por espera explícita
            IWebElement celdaEstado = utilities.WaitForElementToBeVisible(celdaEstadoPrimeraFila);
            string estadoActual = celdaEstado.Text;
            Assert.AreEqual(estadoEsperado, estadoActual.Trim(),
                $"El estado del gasto no es el correcto. Esperado: '{estadoEsperado}', Actual: '{estadoActual}'");
        }

        public void VerificarGastoEnGrilla(string importe, string proveedor)
        {
            // Reemplazamos Thread.Sleep por espera explícita
            utilities.WaitForElementToBeVisible(botonVerEditarPrimeraFila);

            var filas = driver.FindElements(By.XPath("//table[@id='tabla-gastos']/tbody/tr"));
            // ... (el resto de tu lógica está bien) ...
            var filasConDatos = filas.Where(f => !f.Text.Contains("No hay Datos Disponibles")).ToList();
            if (filasConDatos.Count == 0)
            {
                Assert.Fail("La grilla está vacía. No se encontró el gasto.");
            }
            var primeraFila = filasConDatos.First();
            var celdas = primeraFila.FindElements(By.TagName("td"));
            bool encontrado = false;

            if (!string.IsNullOrEmpty(importe))
            {
                string importeEnGrilla = celdas[6].Text;
                string importeLimpio = importeEnGrilla.Replace(",", "").Replace(".00", "").Trim();
                if (importeLimpio == importe)
                {
                    encontrado = true;
                }
            }
            else if (!string.IsNullOrEmpty(proveedor))
            {
                string proveedorEnGrilla = celdas[5].Text;
                if (proveedorEnGrilla == proveedor)
                {
                    encontrado = true;
                }
            }
            Assert.IsTrue(encontrado,
                $"El gasto no se verificó correctamente. Se buscaba Importe='{importe}' o Proveedor='{proveedor}'.\nValores encontrados: Proveedor='{celdas[5].Text}', Importe='{celdas[6].Text}'");
        }

        // --- ¡NUEVOS MÉTODOS AÑADIDOS! ---

        /// <summary>
        /// Verifica que el modal "DETALLE DE GASTO" esté visible.
        /// </summary>
        public void VerificarModalDetalleGasto()
        {
            IWebElement titulo = utilities.WaitForElementToBeVisible(tituloModalDetalle);
            Assert.IsTrue(titulo.Displayed, "El modal 'DETALLE DE GASTO' no apareció.");
        }

        /// <summary>
        /// Cierra el modal de "DETALLE DE GASTO".
        /// </summary>
        public void CerrarModalDetalle()
        {
            utilities.ClickButton(botonCerrarModalDetalle);
        }
    }
}