import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { ConsequenceList } from "../../components/ConsequenceList"
import type { Consequence } from "../../types/consequence"

const mockConsequences: Consequence[] = [
  { id: "1", name: "Timeout", description: "Brief removal from activity" },
  { id: "2", name: "Verbal praise", description: "Positive verbal feedback" },
]

describe("ConsequenceList", () => {
  const onAdd = jest.fn()
  const onEdit = jest.fn()
  const onDelete = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders the heading", () => {
    render(
      <ConsequenceList
        consequences={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.getByText("Consequences")).toBeInTheDocument()
  })

  it("renders all consequences", () => {
    render(
      <ConsequenceList
        consequences={mockConsequences}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.getByText("Timeout")).toBeInTheDocument()
    expect(screen.getByText("Verbal praise")).toBeInTheDocument()
  })

  it("calls onAdd when Add button is clicked", () => {
    render(
      <ConsequenceList
        consequences={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    fireEvent.click(screen.getByText("Add Consequence"))
    expect(onAdd).toHaveBeenCalledTimes(1)
  })

  it("calls onEdit with the correct id", () => {
    render(
      <ConsequenceList
        consequences={mockConsequences}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    fireEvent.click(screen.getAllByText("Edit")[0])
    expect(onEdit).toHaveBeenCalledWith("1")
  })

  it("calls onDelete with the correct id", () => {
    render(
      <ConsequenceList
        consequences={mockConsequences}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    fireEvent.click(screen.getAllByText("Delete")[1])
    // Confirm the deletion in the dialog
    const allDeleteBtns = screen.getAllByRole("button", { name: "Delete" })
    fireEvent.click(allDeleteBtns[allDeleteBtns.length - 1])
    expect(onDelete).toHaveBeenCalledWith("2")
  })
})
