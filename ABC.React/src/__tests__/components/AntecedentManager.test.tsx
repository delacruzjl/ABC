import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { AntecedentManager } from "../../components/AntecedentManager"
import type { Antecedent } from "../../types/antecedent"

describe("AntecedentManager", () => {
  const onSave = jest.fn()
  const onCancel = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders Add form when antecedent is null", () => {
    render(
      <AntecedentManager
        antecedent={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByText("Add Antecedent")).toBeInTheDocument()
    expect(screen.getByText("Add")).toBeInTheDocument()
  })

  it("renders Edit form when antecedent is provided", () => {
    const antecedent: Antecedent = {
      id: "1",
      name: "Test",
      description: "Test desc",
    }
    render(
      <AntecedentManager
        antecedent={antecedent}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByText("Edit Antecedent")).toBeInTheDocument()
    expect(screen.getByText("Update")).toBeInTheDocument()
  })

  it("populates form fields from existing antecedent", () => {
    const antecedent: Antecedent = {
      id: "1",
      name: "Loud noise",
      description: "A sudden loud noise",
    }
    render(
      <AntecedentManager
        antecedent={antecedent}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByLabelText("Antecedent Name")).toHaveValue("Loud noise")
    expect(screen.getByLabelText("Antecedent Description")).toHaveValue(
      "A sudden loud noise"
    )
  })

  it("calls onSave with form data on submit", () => {
    render(
      <AntecedentManager
        antecedent={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    fireEvent.change(screen.getByLabelText("Antecedent Name"), {
      target: { value: "New antecedent", name: "name" },
    })
    fireEvent.change(screen.getByLabelText("Antecedent Description"), {
      target: { value: "New description", name: "description" },
    })
    fireEvent.click(screen.getByText("Add"))

    expect(onSave).toHaveBeenCalledTimes(1)
    expect(onSave).toHaveBeenCalledWith(
      expect.objectContaining({
        name: "New antecedent",
        description: "New description",
      })
    )
  })

  it("does not call onSave when name is empty", () => {
    render(
      <AntecedentManager
        antecedent={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    fireEvent.click(screen.getByText("Add"))
    expect(onSave).not.toHaveBeenCalled()
  })

  it("calls onCancel when Cancel is clicked", () => {
    render(
      <AntecedentManager
        antecedent={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    fireEvent.click(screen.getByText("Cancel"))
    expect(onCancel).toHaveBeenCalledTimes(1)
  })

  it("shows saving state when saving prop is true", () => {
    render(
      <AntecedentManager
        antecedent={null}
        onSave={onSave}
        onCancel={onCancel}
        saving={true}
      />
    )
    expect(screen.getByText("Saving…")).toBeInTheDocument()
  })
})
