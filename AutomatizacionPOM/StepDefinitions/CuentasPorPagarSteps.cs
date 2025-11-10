using AutomatizacionPOM.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class CuentasPorPagarSteps
    {
        private readonly IWebDriver driver;
        private readonly CuentasPorPagarPage _cuentasPage;

        public CuentasPorPagarSteps(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            _cuentasPage = new CuentasPorPagarPage(driver);
        }

        // --- Pasos del 'When' (Acciones) ---

        [When(@"el usuario hace clic en el radio button ""Por Pagar""")]
        public void WhenElUsuarioHaceClicEnElRadioButtonPorPagar()
        {
            _cuentasPage.ClickRadioPorPagar();
        }

        [When(@"el usuario selecciona la primera cuota")]
        public void WhenElUsuarioSeleccionaLaPrimeraCuota()
        {
            _cuentasPage.SeleccionarPrimeraCuota();
        }

       

       

        [Then(@"el sistema muestra la advertencia ""([^""]*)""")]
        public void ThenElSistemaMuestraLaAdvertencia(string mensaje)
        {
            _cuentasPage.VerificarAdvertenciaDeClientesDiferentes();
        }
    }
}