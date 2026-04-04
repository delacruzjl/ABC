import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { BehaviorManager } from "../../components/BehaviorManager"
import type { Behavior } from "../../types/behavior"

describe("BehaviorManager", () => {
  const onSave = jest.fn()
  const onCancel = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders Add form when behavior is null", () => {
    render(
      <BehaviorManager behavior={null} onSave={onSave} onCancel={onCancel} />
    )
    expect(screen.getByText("Add Behavior")).toBeInTheDocument()
    expect(screen.getByText("Add")).toBeInTheDocument()
  })

  it("renders Edit form when behavior is provided", () => {
    const behavior: Behavior = {
      id: "1",
      name: "Test",
      description: "Test desc",
    }
    render(
      <BehaviorManager
        behavior={behavior}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByText("Edit Behavior")).toBeInTheDocument()
    expect(screen.getByText("Update")).toBeInTheDocument()
  })

  it("populates form fields from existing behavior", () => {
    const behavior: Behavior = {
      id: "1",
      name: "Hand flapping",
      description: "Repetitive hand movements",
    }
    render(
      <BehaviorManager
        behavior={behavior}
        onSave={onSave}
        onCancel={onCancel}
      />
    )
    expect(screen.getByLabelText("Behavior Name")).toHaveValue("Hand flapping")
    expect(screen.getByLabelText("Behavior Description")).toHaveValue(
      "Repetitive hand movements"
    )
  })

  it("calls onSave with form data on submit", () => {
    render(
      <BehaviorManager behavior={null} onSave={onSave} onCancel={onCancel} />
    )
    fireEvent.change(screen.getByLabelText("Behavior Name"), {
      target: { value: "New behavior", name: "name" },
    })
    fireEvent.change(screen.getByLabelText("Behavior Description"), {
      target: { value: "New description", name: "description" },
    })
    fireEvent.click(screen.getByText("Add"))

    expect(onSave).toHaveBeenCalledWith(
      expect.objectContaining({
        name: "New behavior",
        description: "New description",
      })
    )
  })

  it("does not call onSave when name is empty", () => {
    render(
      <BehaviorManager behavior={null} onSave={onSave} onCancel={onCancel} />
    )
    fireEvent.click(screen.getByText("Add"))
    expect(onSave).not.toHaveBeenCalled()
  })

  it("calls onCancel when Cancel is clicked", () => {
    render(
      <BehaviorManager behavior={null} onSave={onSave} onCancel={onCancel} />
    )
    fireEvent.click(screen.getByText("Cancel"))
    expect(onCancel).toHaveBeenCalledTimes(1)
  })

  it("shows saving state when saving prop is true", () => {
    render(
      <BehaviorManager
        behavior={null}
        onSave={onSave}
        onCancel={onCancel}
        saving={true}
      />
    )
    expect(screen.getByText("Saving…")).toBeInTheDocument()
  })
})
