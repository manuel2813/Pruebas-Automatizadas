using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using NUnit.Framework;

namespace AutomatizacionPOM.Pages
{
    public class IngresoEgresoPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public IngresoEgresoPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // --- Localizadores (Tus XPaths) ---

        // Filtros Superiores
        private By fechaInicialField = By.XPath("//input[@id='dateStar']");
        private By fechaFinalField = By.XPath("//input[@id='dateEnd']");
        private By cobrosRadio = By.XPath("//div[@class='col-xs-12 col-sm-3 col-md-3 col-lg-3']//input[@id='radio1']");
        private By pagosRadio = By.XPath("//div[@class='col-xs-12 col-sm-3 col-md-3 col-lg-3']//input[@id='radio2']");
        private By consultarButton = By.XPath("//button[@title='Consultar']");

        // Botones de Acción
        private By ingresoButton = By.XPath("//button[normalize-space()='INGRESO']");
        private By egresoButton = By.XPath("//button[normalize-space()='EGRESO']");
        private By exportarExcelButton = By.XPath("//span[@class='fa fa-file-excel-o']");

        // Filtros de Grilla
        // (XPath mejorado usando el ID de la tabla 'tabla-cobros-pagos' que se infiere de tu XPath 'select')
        private By buscarGeneralField = By.XPath("//div[@id='tabla-cobros-pagos_filter']//input[@type='search']");
        private By filtroDocumentoField = By.XPath("//th[3]//input[1]");
        private By filtroPagadorField = By.XPath("//th[4]//input[1]");
        private By filasSelect = By.XPath("//select[@name='tabla-cobros-pagos_length']");

        // Grilla (para Asserts)
        private By primeraFila = By.XPath("//table[@id='tabla-cobros-pagos']/tbody/tr[1]");
        private By primeraFilaPagador = By.XPath("//table[@id='tabla-cobros-pagos']/tbody/tr[1]/td[4]");
        private By primeraFilaTipoOp = By.XPath("//table[@id='tabla-cobros-pagos']/tbody/tr[1]/td[6]");


        // --- Métodos de Acción ---

        public void IngresarFechaInicial(string dia)
        {
            utilities.SelectOption(fechaInicialField, dia);
        }

        public void MarcarCheckCobros()
        {
            utilities.ClickButton(cobrosRadio);
        }

        public void MarcarCheckPagos()
        {
            utilities.ClickButton(pagosRadio);
        }

        public void ClickConsultar()
        {
            utilities.ClickButton(consultarButton);
            utilities.WaitForBlockOverlayToDisappear(); // Espera a que cargue la grilla
        }

        public void ClickNuevoIngreso()
        {
            utilities.ClickButton(ingresoButton);
        }

        public void ClickNuevoEgreso()
        {
            utilities.ClickButton(egresoButton);
        }

        public void FiltrarPorPagador(string texto)
        {
            utilities.EnterText(filtroPagadorField, texto);
            utilities.WaitForBlockOverlayToDisappear();
        }

        // --- Métodos de Verificación (Asserts) ---

        public void VerificarGrillaActualizada(string textoEsperado)
        {
            // Espera a que la primera fila sea visible y contenga el texto
            bool seMuestra = utilities.WaitForTextToContain(primeraFilaPagador, textoEsperado, 5);
            Assert.IsTrue(seMuestra, $"La grilla no se actualizó. No se encontró '{textoEsperado}' en la primera fila.");
        }

        public void VerificarSoloCobros()
        {
            // Verifica que la primera fila SÍ contenga "COBRO" o "VENTA"
            bool esCobro = utilities.WaitForTextToContain(primeraFilaTipoOp, "COBRO", 3) ||
                             utilities.WaitForTextToContain(primeraFilaTipoOp, "VENTA", 1);
            Assert.IsTrue(esCobro, "La grilla no se filtró por Cobros.");
        }

        public void VerificarSoloPagos()
        {
            // Verifica que la primera fila SÍ contenga "PAGO" o "GASTO"
            bool esPago = utilities.WaitForTextToContain(primeraFilaTipoOp, "PAGO", 3) ||
                            utilities.WaitForTextToContain(primeraFilaTipoOp, "GASTO", 1);
            Assert.IsTrue(esPago, "La grilla no se filtró por Pagos.");
        }
    }
}