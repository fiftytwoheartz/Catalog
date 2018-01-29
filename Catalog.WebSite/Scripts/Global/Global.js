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
    notificator
        .animate(
            { opacity: .0 },
            4000,
            function () {
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
    showFailure({ title: 'Произошла ошибка ;(', body: 'Что-то пошло не так...' });
}

// ajax to the WEB API module
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

// validation module
function pairPredicate(predicate) {
    return {
        withMessage: function(message) {
            return {
                isTrue: predicate,
                message: message,
                and: function (rpredicate) {
                    return and(this, rpredicate);
                }
            };
        }
    };
}
function and(lpredicate, rpredicate) {
    return pairPredicate(function (value) { return lpredicate.isTrue(value) && rpredicate.isTrue(value); })
        .withMessage(lpredicate.message + ' --[OR]-- ' + rpredicate.message);
}

function createValidator(failWith, approve) {
    return {
        value: {},
        isValid: true,
        failWith: failWith,
        approve: approve,
        validate: function (selector, propertyName, predicate) {
            var elementValue = $(selector).val();
            var withNotNullPredicate = predicate.and(pairPredicate(function (value) { return value != null }).withMessage("Can't be null."));
            if (withNotNullPredicate.isTrue(elementValue)) {
                if (this.isValid) {
                    this.value[propertyName] = elementValue;
                }
                approve(selector);
            } else {
                this.isValid = false;
                this.value = {};
                failWith(selector, withNotNullPredicate.message);
            }
            return this;
        },
        apply: function(func) {
            if (this.isValid) {
                func(this.value);
            }
        }
    };
}

// relies on the fact that each input is wrapped into a separate parent container,
// typically 'div' to give room for error message
var invalidCls = 'alert alert-danger';
var validCls = 'alert alert-success';
var defaultValidator = {
    get: function () {
        return createValidator(
            function(selector, message) {
                var element = $(selector);
                element.removeClass(validCls);
                element.addClass(invalidCls);

                var elementParent = element.parent();
                var errorMessageElement = elementParent.find('small');
                if (errorMessageElement.length === 0) {
                    elementParent.append('<small class="text-danger">' + message + '</small>');
                }
            },
            function(selector) {
                var element = $(selector);
                element.removeClass(invalidCls);
                element.addClass(validCls);
                var errorMessageElement = element.parent().find('small');
                if (errorMessageElement != null) {
                    errorMessageElement.remove();
                }
            });
    }
}

// predicates
var predicates = {
    string: {
        longerThan: function(limitExclusive) {
            return pairPredicate(function(value) { return value.length > limitExclusive })
                    .withMessage("Минимальная длина строки: " + limitExclusive + " символ(а).");
        },
        shorterThan: function (limitExclusive) {
            return pairPredicate(function (value) { return value.length < limitExclusive })
                .withMessage("Максимальная длина строки: " + limitExclusive + " символ(а).");
        }
    }
};
