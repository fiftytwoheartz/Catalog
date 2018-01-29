var successCls = 'alert alert-success';
var failureCls = 'alert alert-danger';

// core module
function notNull(nullValue) {
    return function (value) {
        return {
            object: object,
            map: function (func) {
                var res = func(value);
                if (res == null) {
                    return notNull(nullValue);
                }
                return res;
            }
        };
    };
}

// user notification
function createMsg(title, body) {
    return { title: title, body: body };
}
var defaultSuccessMsg = createMsg('Успех!', 'Все прошло хорошо, можно продолжать работу.');
var defaultFailureMsg = createMsg('Ошибка! ;(', 'Что-то пошло не так...');

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
    if (message == null) {
        message = defaultSuccessMsg;
    }
    show(successCls, message);
}
function showFailure(message) {
    if (message == null) {
        message = defaultFailureMsg;
    }
    show(failureCls, message);
}


// ajax to WEB API module
function makeRequest(hostUrl, httpVerb) {
    return function (
        relativeUrl,
        successMessageFormatter = function (_) { showSuccess(defaultSuccessMsg); },
        failureMessageFormatter = function (_) { showFailure(defaultFailureMsg); },
        successCallback = function (_) { },
        failureCallback = function (_) { },
        errorCallback = function () { }) {
        return function (data) {
            $.ajax({
                url: hostUrl + relativeUrl,
                type: httpVerb,
                data: data,
                success: function (result) {
                    // this part relies on the WEB API contract:
                    // there should be .Success as a confirmation from each and every request
                    if (result.Success) {
                        showSuccess(successMessageFormatter(result));
                        successCallback(result);
                    }
                    else {
                        showFailure(failureMessageFormatter(result));
                        failureCallback(result);
                    }
                },
                error: function () {
                    showFailure(failureMessageFormatter({}));
                    errorCallback();
                }
            });

            return createAjaxMonad(hostUrl);
        };
    }
}

function createAjaxMonad(hostUrl) {
    return {
        post: makeRequest(hostUrl, 'POST'),
        delete: makeRequest(hostUrl, 'DELETE')
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
            return this;
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
                    .withMessage("Минимальная длина строки: " + (limitExclusive + 1) + " символ(а).");
        },
        shorterThan: function (limitExclusive) {
            return pairPredicate(function (value) { return value.length < limitExclusive })
                .withMessage("Максимальная длина строки: " + (limitExclusive - 1) + " символ(а).");
        }
    }
};
