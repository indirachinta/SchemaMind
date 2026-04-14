"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.activate = activate;
const vscode = __importStar(require("vscode"));
const https = __importStar(require("https"));
const node_fetch_1 = __importDefault(require("node-fetch"));
// npm install node-fetch@2
const agent = new https.Agent({
    rejectUnauthorized: false
});
function activate(context) {
    vscode.commands.registerCommand('schemamind.askQuestion', async () => {
        console.log("Command triggered");
        const config = vscode.workspace.getConfiguration('schemamind');
        const connectionString = config.get('connectionString');
        const apiUrl = config.get('apiUrl');
        console.log("Connection:", connectionString);
        console.log("API URL:", apiUrl);
        const question = await vscode.window.showInputBox({
            prompt: "Enter your SQL question"
        });
        console.log("Question:", question);
        if (!question) {
            vscode.window.showWarningMessage("No question entered");
            return;
        }
        vscode.window.showInformationMessage("Calling SchemaMind API...");
        try {
            const response = await (0, node_fetch_1.default)(`${apiUrl}/generate-sql`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    question: question,
                    connectionString: connectionString
                })
            });
            console.log("Response received");
            const data = await response.json();
            console.log("Response data:", data);
            vscode.window.showInformationMessage("SQL Generated!");
            const output = `SQL:\n${data.query}\n\nResults:\n${JSON.stringify(data.results, null, 2)}`;
            const doc = await vscode.workspace.openTextDocument({
                content: output,
                language: 'sql'
            });
            await vscode.window.showTextDocument(doc);
        }
        catch (err) {
            console.error("Error:", err);
            vscode.window.showErrorMessage("Error: " + err.message);
        }
    });
}
//# sourceMappingURL=extension.js.map