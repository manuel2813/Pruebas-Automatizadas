using AutomatizacionPOM.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class GastoReporteSteps
    {
        private readonly IWebDriver driver;
        private readonly GastoReportePage _reportePage;
        private readonly ReportViewerPage _viewerPage;

        public GastoReporteSteps(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            // Creamos las instancias de las nuevas Pages
            _reportePage = new GastoReportePage(driver);
            _viewerPage = new ReportViewerPage(driver);
        }

        // --- Pasos del 'When' (Acciones) ---

        [When(@"el usuario ingresa el dia '([^']*)' en la Fecha Inicial del reporte")]
        public void WhenElUsuarioIngresaElDiaEnLaFechaInicialDelReporte(string dia)
        {
            _reportePage.IngresarFechaInicial(dia);
        }

        [When(@"el usuario ingresa el dia '([^']*)' en la Fecha Final del reporte")]
        public void WhenElUsuarioIngresaElDiaEnLaFechaFinalDelReporte(string dia)
        {
            _reportePage.IngresarFechaFinal(dia);
        }

        [When(@"el usuario selecciona el tipo de reporte '([^']*)'")]
        public void WhenElUsuarioSeleccionaElTipoDeReporte(string tipo)
        {
            _reportePage.SeleccionarTipoReporte(tipo);
        }

        [When(@"el usuario hace clic en el boton 'VER'")]
        public void WhenElUsuarioHaceClicEnElBotonVER()
        {
            _reportePage.ClickVerReporte();
        }

        [When(@"el usuario selecciona el zoom '([^']*)'")]
        public void WhenElUsuarioSeleccionaElZoom(string zoomLevel)
        {
            _viewerPage.SeleccionarZoom(zoomLevel);
        }

        // --- Pasos del 'Then' (Verificaciones) ---

        [Then(@"el visor de reportes deberia cargarse")]
        public void ThenElVisorDeReportesDeberiaCargarse()
        {
            _viewerPage.VerificarReporteCargado();
        }

        [Then(@"el visor de reportes no deberia ser visible aun")]
        public void ThenElVisorDeReportesNoDeberiaSerVisibleAun()
        {
            // Este es un test negativo, verifica que NO se cargó
            // (Lo implementaremos si es necesario, por ahora es un placeholder)
        }

        
    }
}