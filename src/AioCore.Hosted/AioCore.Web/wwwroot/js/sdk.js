Host = 'https://aioc.one';
Constants = {
    Pixel: Host + '/pixel?tenant={0}&event={1}'
};
Enums = {};
Utils = {
    String: {
        format: (template, arguments) => {
            for (const argument in arguments) {
                template = template.replace('{' + argument + '}', arguments[argument]);
            }
            return template;
        },
    },
};
Handlers = {
    events: {
        default(tenant, event) {
            console.log(`
                Tenant: ${tenant},
                Event: ${event}
            `)
        },
        pageView(context) {}
    }
};
Aioc = {
    init(context) {
        const {tenant, event} = context;
        switch (event) {
            case 'default':
                Handlers.events.default(tenant, event);
                break;
            case 'page-view':
                break;
        }
    }
};