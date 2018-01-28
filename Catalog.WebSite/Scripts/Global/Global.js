var successCls = 'alert alert-success';
var failureCls = 'alert alert-error';

function show(cls, message) {
    var notificator = $("#notificator-container");
    notificator.html(
        "<p>" +
        "<strong>" + message.title + "</strong>" +
        "<hr />" +
        message.body +
        "</p>")
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

/* WEB API */
function createMsg(title, body) {
    return { title: title, body: body };
}
var defaultSuccessMsg = createMsg('Успех!', 'Все прошло хорошо, можно продолжать работу.');
var defaultFailureMsg = createMsg('Ошибка! ;(', 'Что-то пошло не так...');
function curryHostAndHttpVerb(hostUrl, httpVerb) {
    return function (relativeUrl, data, success, failure) {
        $.ajax({
            url: hostUrl + relativeUrl,
            type: httpVerb,
            data: data,
            success: function (result) {
                if (result.Success) {
                    if (success != null && success.messageBuilder != null) {
                        showSuccess(success.messageBuilder(result.Data));
                    } else {
                        showSuccess(defaultSuccessMsg);
                    }

                    if (success != null && success.callback != null) {
                        success.callback(result.Data);
                    }
                }
                else {
                    if (failure != null && failure.messageBuilder != null) {
                        showFailure(failure.messageBuilder(result.Data));
                    } else {
                        showFailure(defaultFailureMsg);
                    }

                    if (failure != null && failure.callback != null) {
                        failure.callback(result.Data);
                    }
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