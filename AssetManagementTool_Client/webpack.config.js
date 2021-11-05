const path = require('path');
module.exports = {
    entry: ['./wwwroot/js/common.js',
        './wwwroot/js/IFCInterop.js'
    ],
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        filename: 'bundle.js'
    }
};