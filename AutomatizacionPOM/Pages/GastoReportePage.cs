using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;

namespace AutomatizacionPOM.Pages
{
    public class GastoReportePage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public GastoReportePage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // --- Localizadores (Tus XPaths) ---

        // Filtros de Fecha
        private By fechaInicialField = By.XPath("//input[@id='dateStart']");
        private By fechaFinalField = By.XPath("//input[@id='dateEnd']");

        // Tipos de Reporte
        private By globalRadio = By.XPath("//input[@id='radio1']");
        private By establecimientoRadio = By.XPath("//input[@id='radio2']");
        private By centroAtencionRadio = By.XPath("//input[@id='radio3']");

        // Botón
        private By verReporteButton = By.XPath("//a[normalize-space()='VER']");


        // --- Métodos de Acción ---

        public void IngresarFechaInicial(string dia)
        {
            // Reutilizamos la utilidad que ya funciona
            utilities.SelectOption(fechaInicialField, dia);
        }

        public void IngresarFechaFinal(string dia)
        {
            utilities.SelectOption(fechaFinalField, dia);
        }

        public void SeleccionarTipoReporte(string tipo)
        {
            // Hacemos clic en el radio button correspondiente
            switch (tipo.ToLower())
            {
                case "global":
                    utilities.ClickButton(globalRadio);
                    break;
                case "establecimiento":
                    utilities.ClickButton(establecimientoRadio);
                    break;
                case "centro de atención":
                    utilities.ClickButton(centroAtencionRadio);
                    break;
                default:
                    throw new System.Exception("Tipo de reporte no válido: " + tipo);
            }
        }

        public void ClickVerReporte()
        {
            utilities.ClickButton(verReporteButton);
            // Damos tiempo extra para que el visor de reportes cargue
            Thread.Sleep(5000);
        }
    }
}