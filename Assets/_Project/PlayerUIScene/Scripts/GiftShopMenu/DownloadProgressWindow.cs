using Paroxe.PdfRenderer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class DownloadProgressWindow : MonoBehaviour
    {
        [SerializeField] private PDFViewer _pdfViewer;
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private GameObject _loadingText;
        [SerializeField] private GameObject _loadingFailedText;
        [SerializeField] private Image _progressImage;
        [SerializeField] private Image _progressImageBackground;
        [SerializeField] private Color _loadingColor;
        [SerializeField] private Color _failedColor;

        [HideInInspector] public UnityEvent OnLoadBook = new();

        private void OnEnable()
        {
            _pdfViewer.OnProgressChanged.AddListener(OnProgressChanged);
            _pdfViewer.OnDocumentLoadFailed += OnDocumentLoadFailed;
            _pdfViewer.OnDocumentLoaded += OnDocumentLoaded;
            _tryAgainButton.onClick.AddListener(LoadBook);
        }

        private void Start()
        {
            ResetVisual(true);
        }

        private void OnDisable()
        {
            _pdfViewer.OnProgressChanged.RemoveListener(OnProgressChanged);
            _pdfViewer.OnDocumentLoadFailed -= OnDocumentLoadFailed;
            _pdfViewer.OnDocumentLoaded -= OnDocumentLoaded;
            _tryAgainButton.onClick.RemoveListener(LoadBook);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetVisual(true);
        }

        private void OnProgressChanged(float progress)
        {
            _progressImage.fillAmount = progress;
        }

        private void OnDocumentLoadFailed(PDFViewer sender)
        {
            ResetVisual(false);
        }

        private void OnDocumentLoaded(PDFViewer sender, PDFDocument document)
        {
            gameObject.SetActive(false);
        }

        private void LoadBook()
        {
            OnLoadBook?.Invoke();
        }

        private void ResetVisual(bool isLoading)
        {
            _tryAgainButton.gameObject.SetActive(!isLoading);
            _loadingText.SetActive(isLoading);
            _loadingFailedText.SetActive(!isLoading);
            _progressImage.color = isLoading ? _loadingColor : _failedColor;
            _progressImageBackground.color = isLoading ? _loadingColor : _failedColor;

            float fillAmount = _progressImage.fillAmount;
            _progressImage.fillAmount = isLoading ? 0f : fillAmount;
        }
    }
}