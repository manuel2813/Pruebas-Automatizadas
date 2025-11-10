using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using NUnit.Framework;

namespace AutomatizacionPOM.Pages
{
    public class ReportViewerPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public ReportViewerPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // --- Localizadores (Tus XPaths) ---
        private By paginaActualInput = By.XPath("//input[@id='ReportViewer1_ctl09_ctl00_CurrentPage']");

        // IDs correctos de tu captura 'image_0b0937.png'
        private By exportButton = By.XPath("//span[@id='ReportViewer1_ctl09_ctl04_ctl00_ButtonImg']");
        private By printButton = By.XPath("//span[@id='ReportViewer1_ctl09_ctl05_ctl00_ButtonImg']");

        private By searchInput = By.XPath("//input[@id='ReportViewer1_ctl09_ctl03_ctl00']");
        private By zoomSelect = By.XPath("//select[@id='ReportViewer1_ctl09_ctl02_ctl00']");


        // --- Métodos de Verificación (Asserts) ---

        public void VerificarReporteCargado()
        {
            // Verificamos que el visor (ej. el input de página) esté visible
            bool estaVisible = utilities.WaitForTextToBeVisible(paginaActualInput, 15); // Espera 15 seg
            Assert.IsTrue(estaVisible, "El visor de reportes (ReportViewer) no cargó a tiempo.");
        }

        public void SeleccionarZoom(string zoomLevel)
        {
            // zoomLevel debe ser "100%", "150%", etc.
            utilities.SelectOption(zoomSelect, zoomLevel);
        }

    }

}
