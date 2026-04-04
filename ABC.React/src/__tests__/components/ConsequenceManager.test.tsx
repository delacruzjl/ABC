import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { ConsequenceManager } from "../../components/ConsequenceManager"
import type { Consequence } from "../../types/consequence"

describe("ConsequenceManager", () => {
  const onSave = jest.fn()
  const onCancel = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders Add form when consequence is null", () => {
    render(
      <ConsequenceManager
        consequence={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByText("Add Consequence")).toBeInTheDocument()
    expect(screen.getByText("Add")).toBeInTheDocument()
  })

  it("renders Edit form when consequence is provided", () => {
    const consequence: Consequence = {
      id: "1",
      name: "Test",
      description: "Test desc",
    }
    render(
      <ConsequenceManager
        consequence={consequence}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByText("Edit Consequence")).toBeInTheDocument()
    expect(screen.getByText("Update")).toBeInTheDocument()
  })

  it("populates form fields from existing consequence", () => {
    const consequence: Consequence = {
      id: "1",
      name: "Timeout",
      description: "Brief removal",
    }
    render(
      <ConsequenceManager
        consequence={consequence}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByLabelText("Consequence Name")).toHaveValue("Timeout")
    expect(screen.getByLabelText("Consequence Description")).toHaveValue(
      "Brief removal"
    )
  })

  it("calls onSave with form data on submit", () => {
    render(
      <ConsequenceManager
        consequence={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    fireEvent.change(screen.getByLabelText("Consequence Name"), {
      target: { value: "New consequence", name: "name" },
    })
    fireEvent.change(screen.getByLabelText("Consequence Description"), {
      target: { value: "New description", name: "description" },
    })
    fireEvent.click(screen.getByText("Add"))

    expect(onSave).toHaveBeenCalledWith(
      expect.objectContaining({
        name: "New consequence",
        description: "New description",
      })
    )
  })

  it("does not call onSave when name is empty", () => {
    render(
      <ConsequenceManager
        consequence={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    fireEvent.click(screen.getByText("Add"))
    expect(onSave).not.toHaveBeenCalled()
  })

  it("calls onCancel when Cancel is clicked", () => {
    render(
      <ConsequenceManager
        consequence={null}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    fireEvent.click(screen.getByText("Cancel"))
    expect(onCancel).toHaveBeenCalledTimes(1)
  })

  it("shows saving state when saving prop is true", () => {
    render(
      <ConsequenceManager
        consequence={null}
        onSave={onSave}
        onCancel={onCancel}
        saving={true}
      />
    )
    expect(screen.getByText("Saving…")).toBeInTheDocument()
  })
})
