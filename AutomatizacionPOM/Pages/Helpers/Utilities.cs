using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; // <-- Using para WebDriverWait
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
// IMPORTANTE: Este paquete te da las "ExpectedConditions"
// Si te da error, instálalo desde NuGet: DotNetSeleniumExtras.WaitHelpers
using SeleniumExtras.WaitHelpers;

namespace AutomatizacionPOM.Pages.Helpers
{
    public class Utilities
    {
        private IWebDriver driver;
        private WebDriverWait wait; // Objeto de espera explícita

        // Localizador para el overlay que te da problemas
        private By blockOverlay = By.ClassName("block-ui-overlay");

        public Utilities(IWebDriver driver)
        {
            this.driver = driver;
            // Inicializamos el wait para que espere un MÁXIMO de 20 segundos
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        // --- MÉTODOS DE ESPERA ---

        /// <summary>
        /// Espera a que el overlay "block-ui-overlay" desaparezca.
        /// </summary>
        public void WaitForBlockOverlayToDisappear()
        {
            try
            {
                // Espera hasta que el elemento overlay NO sea visible
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(blockOverlay));
            }
            catch (Exception)
            {
                // Si el overlay nunca apareció o ya había desaparecido, no hay problema.
            }
        }

        /// <summary>
        /// Espera a que un elemento sea visible y lo devuelve
        /// </summary>
        public IWebElement WaitForElementToBeVisible(By locator)
        {
            // Primero, esperamos que cualquier carga (overlay) termine
            WaitForBlockOverlayToDisappear();
            // Luego, esperamos que el elemento específico sea visible
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        // --- MÉTODOS DE ACCIÓN (TODOS LIMPIOS) ---

        /// <summary>
        /// CORREGIDO: Espera overlay y espera que sea clickeable
        /// </summary>
        public void ClickButton(By _path)
        {
            // 1. Espera a que el overlay desaparezca
            WaitForBlockOverlayToDisappear();

            // 2. Espera a que el elemento sea clickeable
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(_path));

            // 3. Intenta el clic normal
            try
            {
                element.Click();
            }
            // 4. Si es interceptado, usa el "clic forzado"
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine("Clic normal interceptado. Forzando con JavaScript.");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", element);
            }
        }

        public void ClickBootstrapButton(By _path)
        {
            // 1. Espera a que el overlay desaparezca
            WaitForBlockOverlayToDisappear();

            // 2. Espera a que el elemento sea clickeable
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(_path));

            // 3. Damos una mini-pausa extra por si acaso
            Thread.Sleep(200);

            // 4. Hacemos el clic NATIVO.
            element.Click();
        }

        /// <summary>
        /// CORREGIDO: Espera overlay y espera que sea visible antes de escribir
        /// </summary>
        public void EnterText(By _path, string _field)
        {
            IWebElement element = WaitForElementToBeVisible(_path); // Ya espera al overlay
            element.SendKeys(_field);
        }

        /// <summary>
        /// CORREGIDO: Limpia y escribe usando esperas
        /// </summary>
        public void ClearAndEnterText(By _path, string _field)
        {
            IWebElement element = WaitForElementToBeVisible(_path);
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(_field);
        }

        /// <summary>
        /// CORREGIDO: Espera antes de presionar Enter
        /// </summary>
        public void Enter(By _path)
        {
            IWebElement element = WaitForElementToBeVisible(_path);
            element.SendKeys(Keys.Enter);
        }

        /// <summary>
        /// CORREGIDO: Sin Thread.Sleep
        /// </summary>
        public void SelectOption(By pathComponent, string option)
        {
            WaitForBlockOverlayToDisappear();

            // 1. Clic en el dropdown
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(pathComponent));
            dropdown.Click();

            // 2. Clic en la opción
            By optionElementPath = By.XPath($"//li[contains(text(), '{option}')]");
            IWebElement optionElement = wait.Until(ExpectedConditions.ElementToBeClickable(optionElementPath));
            optionElement.Click();
        }

        // --- ¡MÉTODO QUE FALTABA, AÑADIDO Y MEJORADO! ---
        /// <summary>
        /// Para desplegables tipo Select2 (que abren un campo de búsqueda)
        /// </summary>
        public void SelectOptionBySearch(By pathComponent, string optionText)
        {
            try
            {
                // 1. Clic en el contenedor (ya espera al overlay y a ser clickeable)
                ClickButton(pathComponent);

                // 2. XPath para el campo de búsqueda VISIBLE
                By searchField = By.XPath("//body/span[contains(@class,'select2-container--open')]//input[@class='select2-search__field']");

                // 3. Escribimos (ya espera al overlay Y a que sea visible)
                EnterText(searchField, optionText);

                // 4. Presionamos Enter (ya espera)
                Enter(searchField);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al seleccionar (por búsqueda) la opción '{optionText}'. Detalle: {ex.Message}");
                throw;
            }
        }

        // SCROLL (Sin Thread.Sleep, no es necesario)
        public void ScrollViewElement(IWebElement _path)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", _path);
        }

        public void ScrollViewTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, 0);");
        }

        public void WaitForPageLoad()
        {
            try
            {
                // Espera un máximo de 30 segundos a que la página esté "completa"
                var jsWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                jsWait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception)
            {
                Console.WriteLine("Timeout esperando a que document.readyState sea 'complete'.");
            }
        }

        // --- AÑADIMOS DE VUELTA LOS MÉTODOS DE ASSERT QUE CREAMOS ---

        public bool WaitForTextToBeVisible(By _path, int timeOutInSeconds = 5)
        {
            try
            {
                // Usamos un wait local con el timeout que nos pasen
                var localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOutInSeconds));
                localWait.Until(ExpectedConditions.InvisibilityOfElementLocated(blockOverlay));
                localWait.Until(ExpectedConditions.ElementIsVisible(_path));
                return true; // Si lo encuentra, devuelve true
            }
            catch (Exception)
            {
                return false; // Si da timeout, devuelve false
            }
        }

        public bool IsElementEnabled(By _path)
        {
            // Espera a que sea visible (no necesariamente clickeable)
            IWebElement element = WaitForElementToBeVisible(_path);
            return element.Enabled;
        }

        public bool WaitForTextToContain(By _path, string texto, int timeOutInSeconds = 5)
        {
            try
            {
                var localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOutInSeconds));
                localWait.Until(ExpectedConditions.InvisibilityOfElementLocated(blockOverlay));
                // Espera hasta que el texto esté presente en el elemento
                localWait.Until(ExpectedConditions.TextToBePresentInElementLocated(_path, texto));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}