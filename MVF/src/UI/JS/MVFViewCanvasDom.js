window.MVF = window.MVF || {};

window.MVF.dom = {
    _handles: new Map(),
    _nextHandleId: 1,

    _findElementByNodeId: function (nodeId) {
        const escapedNodeId = String(nodeId).replaceAll('\\', '\\\\').replaceAll('"', '\\"');
        const targetByDataUiId = document.querySelector('[data-ui-id="' + escapedNodeId + '"]');

        if (targetByDataUiId) {
            return targetByDataUiId;
        }

        const targetById = document.getElementById(String(nodeId));

        if (targetById) {
            return targetById;
        }

        return null;
    },

    _registerHandle: function (targetElement) {
        const handleId = this._nextHandleId++;
        this._handles.set(handleId, {
            element: targetElement,
            context: null
        });
        return handleId;
    },

    findNodeHandle: function (nodeId) {
        const targetElement = this._findElementByNodeId(nodeId);

        if (!targetElement) {
            throw new Error('MVF target was not found: ' + nodeId);
        }

        return this._registerHandle(targetElement);
    },

    setContext: function (handleId, contextValue) {
        const targetEntry = this._handles.get(handleId);

        if (!targetEntry) {
            throw new Error('MVF handle was not found: ' + handleId);
        }

        targetEntry.context = contextValue;
    },

    getContext: function (handleId) {
        const targetEntry = this._handles.get(handleId);

        if (!targetEntry) {
            throw new Error('MVF handle was not found: ' + handleId);
        }

        return targetEntry.context;
    },

    loadStyle: function (styleUri) {
        const linkElement = document.createElement('link');
        linkElement.rel = 'stylesheet';
        linkElement.href = styleUri;
        document.head.appendChild(linkElement);
    },

    loadJS: function (jsUri) {
        const scriptElement = document.createElement('script');
        scriptElement.src = jsUri;
        scriptElement.defer = true;
        document.head.appendChild(scriptElement);
    },

    setHtml: function (handleId, viewHtml) {
        const targetEntry = this._handles.get(handleId);

        if (!targetEntry) {
            throw new Error('MVF handle was not found: ' + handleId);
        }

        targetEntry.element.innerHTML = viewHtml;
    },

    appendHtml: function (handleId, viewHtml) {
        const targetEntry = this._handles.get(handleId);

        if (!targetEntry) {
            throw new Error('MVF handle was not found: ' + handleId);
        }

        targetEntry.element.insertAdjacentHTML('beforeend', viewHtml);
    }
};
