# IonShard - API Reference

**Base URL:** `http://localhost:5000`

**Content-Type:** `application/json`

---

## Authentication

IonShard uses HTTP Basic authentication with a role system. Credentials are defined in server configuration.

| Role | Access |
|---|---|
| _(none)_ | Read operations, user creation, unit movement |
| `Admin` | Resource quantity management, direct unit placement |
| `Shard` | Cross-shard unit injection with explicit position and resources |

Include credentials as a Basic auth header:

```
Authorization: Basic <base64(username:password)>
```

---

## Error Responses

| Status | Condition |
|---|---|
| 400 Bad Request | Invalid request body, missing required field, constraint violation |
| 401 Unauthorized | Action requires a role the caller does not hold |
| 404 Not Found | User, unit, building, system, or planet does not exist |
| 308 Permanent Redirect | Unit successfully transferred to another shard (inter-shard jump) |
| 502 Bad Gateway | Target shard unreachable during an inter-shard jump |

Error body:

```json
{
  "error": "Descriptive message"
}
```

---

## Systems

### `GET /Systems`

Returns all star systems and their planets.

No auth required.

**Response 200:**

```json
[
  {
    "name": "alpha-centauri",
    "planets": [
      { "name": "proxima-b", "size": 3 }
    ]
  }
]
```

---

### `GET /Systems/{systemName}`

Returns a single star system and all its planets.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `systemName` | string | Name of the star system |

**Response 200:**

```json
{
  "name": "alpha-centauri",
  "planets": [
    { "name": "proxima-b", "size": 3 },
    { "name": "proxima-c", "size": 7 }
  ]
}
```

**Response 404:** System not found.

---

### `GET /Systems/{systemName}/planets`

Returns all planets in a system.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `systemName` | string | Name of the star system |

**Response 200:**

```json
[
  { "name": "proxima-b", "size": 3 },
  { "name": "proxima-c", "size": 7 }
]
```

**Response 404:** System not found.

---

### `GET /Systems/{systemName}/planets/{planetName}`

Returns a single planet.

**Path parameters:**

| Name | Type | Description |
|---|---|---|
| `systemName` | string | Name of the star system |
| `planetName` | string | Name of the planet |

**Response 200:**

```json
{ "name": "proxima-b", "size": 3 }
```

**Response 404:** System or planet not found.

---

## Users

### `GET /Users/{userId}`

Returns an existing user.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | Alphanumeric user ID (`[a-zA-Z0-9_-]+`) |

**Response 200:**

```json
{
  "id": "fanatic42",
  "pseudo": "FanaticPilot",
  "dateOfCreation": "2024-05-26T14:32:00",
  "resourcesQuantity": {
    "carbon": 5,
    "iron": 2,
    "gold": 1
  }
}
```

**Response 404:** User not found.

---

### `PUT /Users/{userId}`

Creates a new user, or updates resource quantities for an existing user.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | Must match `body.id`. Alphanumeric (`[a-zA-Z0-9_-]+`). |

**Request body:**

```json
{
  "id": "fanatic42",
  "pseudo": "FanaticPilot",
  "dateOfCreation": null,
  "resourcesQuantity": null
}
```

| Field | Type | Required | Notes |
|---|---|---|---|
| `id` | string | Yes | Must match path `userId` |
| `pseudo` | string | Yes | Display name |
| `dateOfCreation` | datetime | No | `Shard` role only: sets creation timestamp for migrated users |
| `resourcesQuantity` | object | No | `Admin` role only: replaces resource quantities |

**Response 200:** Returns the `UserDTO` (new or existing).

**Response 400:** `id` does not match path, invalid characters in `userId`, or invalid resource key (Admin path).

---

## Units

### `GET /Users/{userId}/units`

Returns all units owned by a user.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |

**Response 200:**

```json
[
  {
    "id": "unit-abc123",
    "type": "scout",
    "system": "alpha-centauri",
    "planet": null,
    "destinationSystem": "kepler-452",
    "destinationPlanet": null,
    "estimatedTimeOfArrival": "2024-05-26T15:00:00",
    "health": 100,
    "resourcesQuantity": null
  }
]
```

**Response 404:** User not found.

---

### `GET /Users/{userId}/units/{unitId}`

Returns a single unit. If the unit is traveling and arrival is within the configured wait threshold, the response waits for arrival before returning.

**Path parameters:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |
| `unitId` | string | Unit ID |

**Response 200:** `UnitDTO` (see above).

**Response 404:** User or unit not found.

---

### `PUT /Users/{userId}/units/{unitId}`

Moves an existing unit, or creates a new unit (requires `Admin` or `Shard` role).

**Path parameters:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |
| `unitId` | string | Must match `body.id` |

**Request body:**

```json
{
  "id": "unit-abc123",
  "system": null,
  "planet": null,
  "destinationSystem": "kepler-452",
  "destinationPlanet": "kepler-452b",
  "destinationShard": null,
  "type": null,
  "health": 0,
  "resourcesQuantity": null
}
```

| Field | Type | Required | Notes |
|---|---|---|---|
| `id` | string | Yes | Must match path `unitId` |
| `destinationSystem` | string | Yes (move) | Target system name |
| `destinationPlanet` | string | No | Target planet, or null to orbit |
| `destinationShard` | string | No | If set, triggers inter-shard jump (returns 308) |
| `type` | string | Admin/Shard | Unit type for creation (e.g. `scout`, `cargo`, `builder`, `fighter`) |
| `system` | string | Admin | Admin: current system for direct placement |
| `planet` | string | Admin | Admin: current planet for direct placement |
| `health` | int | No | Shard: health override for incoming unit |
| `resourcesQuantity` | object | Cargo | Required when moving a cargo unit |

**Response 200:** `UnitDTO`.

**Response 308 Permanent Redirect:** Unit transferred to target shard. `Location` header contains the destination URL.

**Response 400:** `id` mismatch, missing destination, unknown unit type, or cargo resource error.

**Response 401:** Creating a unit requires `Admin` or `Shard` role.

**Response 404:** User, destination system, or destination planet not found.

**Response 502:** Target shard unreachable during inter-shard jump.

---

### `GET /Users/{userId}/units/{unitId}/location`

Returns detailed location information for a unit, including resources available at the planet if the unit is landed.

**Path parameters:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |
| `unitId` | string | Unit ID |

**Response 200:**

```json
{
  "system": "alpha-centauri",
  "planet": "proxima-b",
  "resourcesQuantity": {
    "carbon": 12,
    "iron": 4
  }
}
```

**Response 404:** User or unit not found.

---

## Buildings

### `POST /Users/{userId}/buildings`

Creates a building at the current location of a builder unit.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |

**Request body:**

```json
{
  "id": "building-xyz789",
  "type": "mine",
  "resourceCategory": "ferrous",
  "builderId": "unit-abc123"
}
```

| Field | Type | Required | Notes |
|---|---|---|---|
| `id` | string | Yes | Building ID |
| `type` | string | Yes | `mine` or `starport` |
| `resourceCategory` | string | Yes (mine) | e.g. `ferrous`, `gaseous`, `liquid`, `solid`, `radioactive` |
| `builderId` | string | Yes | ID of a builder unit currently on a planet |

**Response 201:** `BuildingDTO`.

**Response 400:** Missing field, invalid type, invalid resource category, or builder not on a planet.

**Response 404:** User not found.

---

### `GET /Users/{userId}/buildings`

Returns all buildings owned by a user.

**Path parameter:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |

**Response 200:**

```json
[
  {
    "id": "building-xyz789",
    "type": "mine",
    "system": "alpha-centauri",
    "planet": "proxima-b",
    "isBuilt": false,
    "estimatedBuildTime": "2024-05-26T15:10:00",
    "resourceCategory": "ferrous"
  }
]
```

**Response 404:** User not found.

---

### `GET /Users/{userId}/buildings/{buildingId}`

Returns a single building. If the building is under construction and completion is within the configured wait threshold, the response waits before returning.

**Path parameters:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |
| `buildingId` | string | Building ID |

**Response 200:** `BuildingDTO`.

**Response 404:** User or building not found, or build task failed.

---

### `POST /Users/{userId}/buildings/{starportId}/queue`

Adds a unit to the build queue of a completed starport. Consumes user resources and returns the new unit immediately.

**Path parameters:**

| Name | Type | Description |
|---|---|---|
| `userId` | string | User ID |
| `starportId` | string | ID of a built starport building |

**Request body:**

```json
{
  "type": "scout"
}
```

**Response 200:** `UnitDTO` of the newly created unit.

**Response 400:** Building is not a starport, starport not yet built, unknown unit type, or insufficient resources.

**Response 404:** User or building not found.
