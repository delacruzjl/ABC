import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { AntecedentList } from "../../components/AntecedentList"
import type { Antecedent } from "../../types/antecedent"

const mockAntecedents: Antecedent[] = [
  { id: "1", name: "Loud noise", description: "A sudden loud noise" },
  { id: "2", name: "Crowded room", description: "Being in a crowded space" },
]

describe("AntecedentList", () => {
  const onAdd = jest.fn()
  const onEdit = jest.fn()
  const onDelete = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders the heading", () => {
    render(
      <AntecedentList
        antecedents={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.getByText("Antecedents")).toBeInTheDocument()
  })

  it("renders all antecedents", () => {
    render(
      <AntecedentList
        antecedents={mockAntecedents}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.getByText("Loud noise")).toBeInTheDocument()
    expect(screen.getByText("A sudden loud noise")).toBeInTheDocument()
    expect(screen.getByText("Crowded room")).toBeInTheDocument()
  })

  it("renders the Add button and calls onAdd when clicked", () => {
    render(
      <AntecedentList
        antecedents={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    const addButton = screen.getByText("Add Antecedent")
    fireEvent.click(addButton)
    expect(onAdd).toHaveBeenCalledTimes(1)
  })

  it("calls onEdit with the correct id", () => {
    render(
      <AntecedentList
        antecedents={mockAntecedents}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    const editButtons = screen.getAllByText("Edit")
    fireEvent.click(editButtons[0])
    expect(onEdit).toHaveBeenCalledWith("1")
  })

  it("calls onDelete with the correct id", () => {
    render(
      <AntecedentList
        antecedents={mockAntecedents}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    const deleteButtons = screen.getAllByText("Delete")
    fireEvent.click(deleteButtons[1])
    // Confirm the deletion in the dialog
    const allDeleteBtns = screen.getAllByRole("button", { name: "Delete" })
    fireEvent.click(allDeleteBtns[allDeleteBtns.length - 1])
    expect(onDelete).toHaveBeenCalledWith("2")
  })

  it("renders empty list without errors", () => {
    render(
      <AntecedentList
        antecedents={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.queryByRole("listitem")).not.toBeInTheDocument()
  })
})
