using AutomatizacionPOM.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class CuentasBancariasSteps
    {
        private readonly IWebDriver driver;
        private readonly CuentasBancariasPage _cuentasPage;

        public CuentasBancariasSteps(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            _cuentasPage = new CuentasBancariasPage(driver);
        }

        // --- Pasos del 'When' (Acciones) ---

        [When(@"el usuario hace clic en el boton ""AGREGAR CUENTA BANCARIA""")]
        public void WhenElUsuarioHaceClicEnElBotonAGREGARCUENTABANCARIA()
        {
            _cuentasPage.ClickAgregarCuenta();
        }

        [When(@"el usuario selecciona ""([^""]*)"" como Tipo de Cuenta en la fila (\d+)")]
        public void WhenElUsuarioSeleccionaComoTipoDeCuentaEnLaFila(string tipo, int fila)
        {
            _cuentasPage.SeleccionarTipoCuenta(fila, tipo);
        }

        [When(@"el usuario selecciona ""([^""]*)"" como Entidad Financiera en la fila (\d+)")]
        public void WhenElUsuarioSeleccionaComoEntidadFinancieraEnLaFila(string entidad, int fila)
        {
            _cuentasPage.SeleccionarEntidad(fila, entidad);
        }

        [When(@"el usuario ingresa ""([^""]*)"" como Titular en la fila (\d+)")]
        public void WhenElUsuarioIngresaComoTitularEnLaFila(string titular, int fila)
        {
            _cuentasPage.IngresarTitular(fila, titular);
        }

        [When(@"el usuario selecciona ""([^""]*)"" como Moneda en la fila (\d+)")]
        public void WhenElUsuarioSeleccionaComoMonedaEnLaFila(string moneda, int fila)
        {
            _cuentasPage.SeleccionarMoneda(fila, moneda);
        }

        [When(@"el usuario ingresa ""([^""]*)"" como Numero en la fila (\d+)")]
        public void WhenElUsuarioIngresaComoNumeroEnLaFila(string numero, int fila)
        {
            _cuentasPage.IngresarNumero(fila, numero);
        }

        [When(@"el usuario ingresa ""([^""]*)"" como CCI en la fila (\d+)")]
        public void WhenElUsuarioIngresaComoCCIEnLaFila(string cci, int fila)
        {
            _cuentasPage.IngresarCci(fila, cci);
        }

        [When(@"el usuario hace clic en Guardar de la fila (\d+)")]
        public void WhenElUsuarioHaceClicEnGuardarDeLaFila(int fila)
        {
            _cuentasPage.ClickGuardarFila(fila);
        }

        [When(@"el usuario hace clic en Editar de la fila (\d+)")]
        public void WhenElUsuarioHaceClicEnEditarDeLaFila(int fila)
        {
            _cuentasPage.ClickEditarFila(fila);
        }

        // --- Pasos del 'Then' (Verificaciones) ---

        [Then(@"la cuenta se guarda exitosamente")]
        public void ThenLaCuentaSeGuardaExitosamente()
        {
            _cuentasPage.VerificarGuardadoExitoso();
        }
    }
}