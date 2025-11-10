// ---- Archivo: ConceptoPage.cs (Versión Final Corregida) ----
using AutomatizacionPOM.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Linq;
using System;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class ConceptosPage
    {
        private IWebDriver driver;
        Utilities utilities;
        private WebDriverWait wait;

        public ConceptosPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            // Incrementamos la espera a 20s para ser consistentes con tu error de timeout
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        // --- LOCALIZADORES DE CONSULTA (Página Principal) ---
        private By tituloConsulta = By.XPath("//div[@class='bg-green color-palette']");
        private By campoBuscar = By.XPath("//input[@type='search']");
        private By tablaConceptos = By.Id("tabla-conceptos");
        private By botonEditarPrimeraFila = By.XPath("//table[@id='tabla-conceptos']/tbody/tr[1]//span[@class='glyphicon glyphicon-edit']");

        // --- ¡LOCATOR CORREGIDO! (Basado en tu HTML) ---
        private By botonNuevoConcepto = By.XPath("//a[normalize-space()='NUEVO CONCEPTO']");


        // --- LOCALIZADORES DEL MODAL 'REGISTRAR CONCEPTO' ---

        // El ID del modal (basado en el HTML del botón que lo abre)
        private By modalRegistro = By.Id("modal-registro-concepto-gasto");

        // --- ¡LOCATOR CORREGIDO! (Combobox 'FAMILIA') ---
        private By comboboxFamilia = By.XPath("//span[@aria-labelledby='select2-selectorConceptoBasico-container']");

        // El input que aparece DESPUÉS de hacer clic en el combobox 'FAMILIA'
        private By inputFamilia = By.XPath("//span[@class='select2-container select2-container--default select2-container--open']//input[@class='select2-search__field']");

        // --- ¡LOCATOR CORREGIDO! (Campo 'SUFIJO') ---
        private By campoSufijoModal = By.Id("sufijo");

        // --- ¡LOCATOR CORREGIDO! (Botón 'GUARDAR' del modal) ---
        // Usamos un XPath más seguro que busca el botón DENTRO del modal
        private By botonGuardarModal = By.XPath("//div[@id='modal-registro-concepto-gasto']//button[normalize-space()='GUARDAR']");


        // --- MÉTODOS DE CONSULTA (Página Principal) ---
        public void VerificarPaginaConceptos()
        {
            IWebElement titulo = utilities.WaitForElementToBeVisible(tituloConsulta);
            Assert.IsTrue(titulo.Text.Contains("CONCEPTOS DE GASTOS"), "No se cargó la consulta de Conceptos.");
        }

        public void BuscarConcepto(string nombre)
        {
            utilities.ClearAndEnterText(campoBuscar, nombre);
            utilities.Enter(campoBuscar);
            utilities.WaitForBlockOverlayToDisappear();
        }

        public void VerificarConceptoEnGrilla(string nombre)
        {
            // Espera a que el overlay (si lo hay) desaparezca después de guardar
            utilities.WaitForBlockOverlayToDisappear();
            By celdaConcepto = By.XPath($"//table[@id='tabla-conceptos']//td[normalize-space()='{nombre}']");
            IWebElement celda = utilities.WaitForElementToBeVisible(celdaConcepto);
            Assert.IsTrue(celda.Displayed, $"El concepto '{nombre}' no fue encontrado en la grilla.");
        }

        public void ClicEditarPrimerConcepto()
        {
            utilities.ClickButton(botonEditarPrimeraFila);
        }

        // --- MÉTODOS DEL MODAL DE REGISTRO ---

        public void ClicNuevoConcepto()
        {
            // Clic en el botón "NUEVO CONCEPTO" (el <a>)
            utilities.ClickButton(botonNuevoConcepto);
        }

        public void VerificarModalRegistroVisible()
        {
            // Espera a que el modal esté visible
            IWebElement modal = utilities.WaitForElementToBeVisible(modalRegistro);
            Assert.IsTrue(modal.Displayed, "El modal de Registro de Concepto no apareció.");
        }

        public void EscribirFamiliaEnModal(string nombre)
        {
            // 1. Clic en el 'span' para abrir el input de 'FAMILIA'
            utilities.ClickButton(comboboxFamilia);

            // 2. Esperar y escribir en el input del combobox
            IWebElement input = utilities.WaitForElementToBeVisible(inputFamilia);
            input.SendKeys(nombre);

            // 3. Esperar (mejorado, sin Thread.Sleep)
            // Esperamos a que aparezca la opción resaltada
            By opcionResaltada = By.XPath("//li[contains(@class, 'select2-results__option--highlighted')]");
            utilities.WaitForElementToBeVisible(opcionResaltada);

            // 4. Presionar Enter para seleccionar el nuevo concepto
            input.SendKeys(Keys.Enter);
        }

        public void EscribirSufijoEnModal(string sufijo)
        {
            // Escribe en el campo Sufijo DENTRO DEL MODAL
            utilities.ClearAndEnterText(campoSufijoModal, sufijo);
        }

        public void ClicGuardarEnModal()
        {
            // Clic en el botón Guardar DENTRO DEL MODAL
            utilities.ClickButton(botonGuardarModal);
        }
    }
}