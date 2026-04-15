import * as vscode from 'vscode';
import fetch from 'node-fetch';
import * as https from 'https';

export function activate(context: vscode.ExtensionContext) {

    console.log('SchemaMind extension activated');

    const disposable = vscode.commands.registerCommand('schemamind.askQuestion', async () => {

        const config = vscode.workspace.getConfiguration('schemamind');
        const connectionString = config.get<string>('connectionString');
        const apiUrl = config.get<string>('apiUrl');

        if (!connectionString) {
            vscode.window.showErrorMessage('Please set SchemaMind connection string in settings.');
            return;
        }

        if (!apiUrl) {
            vscode.window.showErrorMessage('Please set SchemaMind API URL in settings.');
            return;
        }

        const question = await vscode.window.showInputBox({
            prompt: 'Enter your SQL question'
        });

        if (!question) {
            vscode.window.showWarningMessage('No question entered.');
            return;
        }

        vscode.window.showInformationMessage('SchemaMind: Generating SQL...');

        // ✅ HTTPS agent (fix for self-signed cert issue)
        const agent = new https.Agent({
            rejectUnauthorized: false
        });

        try {
            const response = await fetch(`${apiUrl}/generate-sql`, {
                method: 'POST',
                agent: agent, // ✅ IMPORTANT FIX
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    question: question,
                    connectionString: connectionString
                })
            });

            if (!response.ok) {
                throw new Error(`API error: ${response.status} ${response.statusText}`);
            }

            const data = await response.json();

            const output = `
-- Generated SQL
${data.query}

-- Results
${JSON.stringify(data.results, null, 2)}
`;

            const doc = await vscode.workspace.openTextDocument({
                content: output,
                language: 'sql'
            });

            await vscode.window.showTextDocument(doc);

            vscode.window.showInformationMessage('SchemaMind: SQL generated successfully');

        } catch (err: any) {
            console.error(err);
            vscode.window.showErrorMessage(`SchemaMind Error: ${err.message}`);
        }
    });

    context.subscriptions.push(disposable);
}

export function deactivate() {}
