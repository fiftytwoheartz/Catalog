var successCls = 'alert alert-success';
var failureCls = 'alert alert-error';

// user notification
function show(cls, message) {
    var notificator = $("#notificator-container");
    notificator.html(
        "<p>" +
        "<strong>" +
        message.title +
        "</strong>" +
        "<hr />" +
        message.body +
        "</p>");
    notificator.addClass(cls);
    notificator.animate({
        opacity: .0,
    }, 4000, function () {
        notificator.removeClass(cls);
        notificator.empty();
        notificator.removeAttr('style');
    });
}

function showSuccess(message) {
    show(successCls, message);
}

function showFailure(message) {
    show(failureCls, message);
}

function showFailure() {
    showFailure({title : 'Произошла ошибка ;(', body : 'Что-то пошло не так...'})
}

/* ajax WEB API */
function createMsg(title, body) {
    return { title: title, body: body };
}
var defaultSuccessMsg = createMsg('Успех!', 'Все прошло хорошо, можно продолжать работу.');
var defaultFailureMsg = createMsg('Ошибка! ;(', 'Что-то пошло не так...');

function processResultAs(result, successOrFailure) {
    if (successOrFailure != null && successOrFailure.messageBuilder != null) {
        showSuccess(successOrFailure.messageBuilder(result.Data));
    } else {
        showSuccess(defaultSuccessMsg);
    }

    if (successOrFailure != null && successOrFailure.callback != null) {
        successOrFailure.callback(result.Data);
    }
}

function process(result) {
    return {
        as: function(obj) {
            processResultAs(obj, result);
        }
    };
}

function curryHostAndHttpVerb(hostUrl, httpVerb) {
    return function (relativeUrl, data, success, failure) {
        $.ajax({
            url: hostUrl + relativeUrl,
            type: httpVerb,
            data: data,
            success: function (result) {
                if (result.Success) {
                    process(result).as(success);
                }
                else {
                    process(result).as(failure);
                }
            },
            failure: function () {
                showFailure(defaultFailureMsg);
            }
        });

        return createAjaxMonad(hostUrl);
    };
}

function createAjaxMonad(hostUrl) {
    return {
        post: curryHostAndHttpVerb(hostUrl, 'POST'),
        delete: curryHostAndHttpVerb(hostUrl, 'DELETE')
    };
}

var ajaxMonad = createAjaxMonad('http://localhost:58798/');

function createValidationResult(isValid, message) {
    return { isValid: isValid, message: message };
}

function createFailedValidationResult(message) {
    return createValidationResult(false, message);
}

function createSuccessfulValidationResult() {
    return createValidationResult(true, null);
}

// validation
function createValidationMonad() {
    return {
        memory: [],
        validate: function(value, validationRules) {
            if (value == null) {
                this.memory.push({
                    validationResult: createFailedValidationResult('Null.'),
                    data: null
                });
                return this;
            } else {
                for (var i = 0; i < validationRules.length; i++) {
                    var currentValidationRule = validationRules[i];
                    if (!currentValidationRule.validate(value)) {
                        this.memory.push({
                            validationResult: createFailedValidationResult(currentValidationRule.message),
                            data: value
                        });
                        return this;
                    }
                };

                this.memory.push({
                    validationResult: createSuccessfulValidationResult(),
                    data: value
                });

                return this;
            }
        }
    };
}
