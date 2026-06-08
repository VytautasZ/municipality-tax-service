---
description: Create a feature branch, commit, push, and open a pull request following the project's VZ convention.
---

# Create commit (VZ convention)

Use this prompt to turn the current working changes into a feature branch, a
single well-formed commit, a push, and a pull request.

## Inputs
- A short task title describing what was done (e.g. `Add unit test project`).
- The current uncommitted changes in the working tree.

## Steps

1. **Determine the next VZ number.**
   - Run `git log --oneline --all` and find the highest existing `VZ-<NN>`.
   - The next number is that value + 1, zero-padded to two digits (`VZ-00` → `VZ-01`).

2. **Create the feature branch** from the up-to-date base branch (`main`):
   ```
   feature/VZ-<NN>_<Short_Title_With_Underscores>
   ```
   - Words separated by underscores, keep it concise.
   - Example: `feature/VZ-01_Add_unit_test_project`

3. **Stage only the relevant changes.**
   - Include source, project, and solution files that belong to this change.
   - Do NOT stage build output (`bin/`, `obj/`), IDE files, or throwaway
     template placeholders (e.g. default `UnitTest1.cs`) unless they are real.
   - Verify with `git diff --cached --name-only` before committing.

4. **Write the commit message** in this exact shape:
   ```
   VZ-<NN> <Imperative verb> <what>

   - <Past-tense statement of what was done>;
   - <Past-tense statement of what was done>;
   - <Past-tense statement of what was done>.
   ```
   - **Subject** = `VZ-<NN> ` + an **imperative verb describing what to do** — one of
     **Add / Change / Remove / Fix** — followed by the target. No trailing period.
     Examples: `VZ-04 Add infrastructure project`, `VZ-05 Fix migration history table`.
   - Blank line between subject and body.
   - **Body** = **bullet points** describing *what is done*, each starting with a
     **past-tense verb** (Added, Changed, Removed, Fixed, Configured, Registered, …)
     and ending with `;` (last bullet ends with `.`).
   - Do NOT add a `Co-Authored-By` / Claude attribution line.
   - When committing from PowerShell use a single-quoted here-string `@'...'@`;
     from bash use two `-m` flags — `-m "<subject>"` and `-m "<bullet body>"` with
     real newlines inside the second quoted string (never mix the two — `@'...'@`
     is not valid bash and leaks a stray `@` into the message).

5. **Push** the branch and set upstream:
   ```
   git push -u origin feature/VZ-<NN>_<Short_Title_With_Underscores>
   ```

6. **Open the pull request** against `main`:
   - Title: same as the commit subject — `VZ-<NN> <Short title>`.
   - Description: the commit body.
   - If the GitHub CLI (`gh`) is available, create it directly; otherwise
     output the `pull/new/...` link from the push for the user to click.

## Example (reference)

Branch: `feature/VZ-04_Add_infrastructure_project`

```
VZ-04 Add infrastructure project

- Added MunicipalityTaxService.Infrastructure project (EF Core 10, SQL Server);
- Added ServiceDbContext, Municipality and TaxRate entity configurations and Db constants;
- Added the Initial migration for the municipalityTax schema;
- Registered infrastructure and applied migrations on startup in the API;
- Added the MunicipalityTaxDbString connection string to appsettings.json.
```
