// customAlerts.js

const showAlert = (type, message, options = {}) => {
    const defaultOptions = {
        autoClose: true,
        timeout: 5000, // 5 seconds
    };

    const mergedOptions = { ...defaultOptions, ...options };

    const alertDiv = $(`
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
    `);

    $('#alert-container').append(alertDiv);

    if (mergedOptions.autoClose) {
        setTimeout(() => {
            alertDiv.alert('close');
        }, mergedOptions.timeout);
    }
};

const showSuccessAlert = (message, options) => {
    showAlert('success', message, options);
};

const showInfoAlert = (message, options) => {
    showAlert('info', message, options);
};

const showWarningAlert = (message, options) => {
    showAlert('warning', message, options);
};

const showErrorAlert = (message, options) => {
    showAlert('danger', message, options);
};
