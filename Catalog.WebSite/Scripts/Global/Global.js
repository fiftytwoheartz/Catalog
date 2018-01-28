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
function createMessageBuilder(title, body) {
    return function () { return { title: title, body: body }; };
}

function createDefaultErrorMessageBuilder() {
    return function () { return { title: 'Ошибка! ;(', body: 'Что-то пошло не так...' }; };
}

function curryHostAndRelativeUrl(host, relativeURL) {
    //return function () {
        return function (optionalParameters, httpVerb, data, success, failure) {
            $.ajax({
                url: host + relativeURL + optionalParameters,
                type: httpVerb,
                data: data,
                success: function (result) {
                    if (result.Success) {
                        showSuccess(success.messageBuilder(result.Data));
                        if (success.callback != null) {
                            success.callback(result.Data);
                        }
                    }
                    else {
                        showFailure(failure.messageBuilder(result.Data));
                        failure.callback(result.Data);
                    }
                },
                failure: function () {
                    showFailure(createDefaultErrorMessageBuilder());
                }
            });
        //};
    };
}

function curryHost(host) {
    return function (relativeUrl) {
        return curryHostAndRelativeUrl(host, relativeUrl);
    };
}