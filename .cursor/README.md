# MongoDB Atlas MCP (this project)

This project uses the **Atlas Managed MCP Server** with a **project-scoped service account** (not the global OAuth plugin session).

## Setup

1. **Create a service account** in your target Atlas project:
   - Atlas → **Project Identity and Access** → **Applications** → **Create Service Account**
   - Grant **Project Owner** (or a role with cluster + data access)
   - Copy the **Client ID** and **Client Secret** (secret is shown once)
   - Add your IP to the service account **API Access List**

2. **Configure credentials locally** (never commit secrets):
   ```bash
   cp .cursor/mcp.json.template .cursor/mcp.json
   ```
   Edit `.cursor/mcp.json` and replace the placeholders with your service account credentials.

3. **Set your cluster target** in `.cursor/atlas.config.json`:
   - `projectId` — 24-character hex Atlas project ID
   - `clusterName` — cluster name in that project
   - `databaseName` — `Leafsteroids` (default for this demo)

4. **Optional — default connection string** (skip `remote-atlas-connect` for routine queries):
   Add to the `env` block in `.cursor/mcp.json`:
   ```json
   "MDB_MCP_CONNECTION_STRING": "mongodb+srv://<user>:<password>@<cluster>.mongodb.net/Leafsteroids"
   ```

5. **Optional — read-only mode**:
   ```json
   "MDB_MCP_READ_ONLY": "true"
   ```

6. **Restart Cursor** fully after saving `.cursor/mcp.json`.

## Avoid duplicate MCP servers

When this project config is active, disable the global **MongoDB Atlas plugin** (OAuth) in **Cursor Settings → Tools & MCP** to avoid two MongoDB MCP servers. Use the project `mongodb-atlas` server defined in `.cursor/mcp.json`.

## Verify

After restart, confirm **Tools & MCP** shows `mongodb-atlas` connected, then:

1. Call `remote-atlas-connect` with `projectId` and `clusterName` from `.cursor/atlas.config.json`
2. Run `list-collections` on database `Leafsteroids`
3. Expect collections such as `config` and `events` (see [README](../README.md))
