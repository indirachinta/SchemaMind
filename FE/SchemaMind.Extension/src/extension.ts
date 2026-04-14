import * as vscode from 'vscode';
import * as https from 'https';
import fetch from 'node-fetch'; 
// npm install node-fetch@2
const agent = new https.Agent({
    rejectUnauthorized: false
});
export function activate(context: vscode.ExtensionContext) {
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
        const response = await fetch(`${apiUrl}/generate-sql`, {
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

    } catch (err: any) {
        console.error("Error:", err);
        vscode.window.showErrorMessage("Error: " + err.message);
    }
});

   
}