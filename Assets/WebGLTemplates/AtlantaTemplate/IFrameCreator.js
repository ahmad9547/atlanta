const createIframeElement = () => {
    const iframeElement = document.createElement("iframe");
    iframeElement.id = "purchase-form-iframe";

    iframeElement.style.width = "100vw";
    iframeElement.style.height = "100vh";

    iframeElement.style.zIndex = "10";

    iframeElement.style.position = "absolute";
    iframeElement.style.top = 0;
    iframeElement.style.left = 0;

    iframeElement.style.background = "white";
    iframeElement.style.border = "none";
    iframeElement.style.outline = "none";

    return iframeElement;
};

const createCloseButton = () => {
    const closeButton = document.createElement("button");
    closeButton.id = "purchase-form-close-button";

    closeButton.style.zIndex = "11";
    closeButton.style.position = "absolute";
    closeButton.style.right = "32px";
    closeButton.style.top = "32px";
    closeButton.style.width = "32px";
    closeButton.style.height = "32px";

    closeButton.addEventListener("mouseover", () => {
        closeButton.style.opacity = "0.3";
    });

    closeButton.addEventListener("mouseleave", () => {
        closeButton.style.opacity = "1";
    });

    const svg = `
        <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M18.7071 5.29289C19.0976 5.68342 19.0976 6.31658 18.7071 6.70711L6.70711 18.7071C6.31658 19.0976 5.68342 19.0976 5.29289 18.7071C4.90237 18.3166 4.90237 17.6834 5.29289 17.2929L17.2929 5.29289C17.6834 4.90237 18.3166 4.90237 18.7071 5.29289Z"
                fill="#000000"
            />
            <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M5.29289 5.29289C5.68342 4.90237 6.31658 4.90237 6.70711 5.29289L18.7071 17.2929C19.0976 17.6834 19.0976 18.3166 18.7071 18.7071C18.3166 19.0976 17.6834 19.0976 17.2929 18.7071L5.29289 6.70711C4.90237 6.31658 4.90237 5.68342 5.29289 5.29289Z"
                fill="#000000"
            />
        </svg>
    `;

    closeButton.insertAdjacentHTML("afterbegin", svg);

    return closeButton;
};

const addElemetToDom = (element) => {
    document.body.appendChild(element);
};

const fillFormInputs = (formdata) => {
    const iframeElement = document.querySelector("#purchase-form-iframe");

    const iframeDocument = iframeElement.contentDocument || iframeElement.contentWindow.document;

    for (const key in formdata) {
        const input = iframeDocument.querySelector(`input[name="${formdata[key].input_name}"]`);

        if (input) {
            input.value = formdata[key].value;
        }
    }
};

const deleteIframe = () => {
    const iframeElement = document.querySelector("#purchase-form-iframe");
    const closeButton = document.querySelector("#purchase-form-close-button");

    iframeElement && iframeElement.remove();
    closeButton && closeButton.remove();
};

const addIframe = (dataJson) => {
    const data = JSON.parse(dataJson);

    const iframeElement = createIframeElement();
    addElemetToDom(iframeElement);

    iframeElement.addEventListener("load", () => {
        fillFormInputs(data.formdata);
    });
    iframeElement.src = data.iframeSrc;

    const closeButton = createCloseButton();
    addElemetToDom(closeButton);

    closeButton.addEventListener("click", () => deleteIframe());
};
