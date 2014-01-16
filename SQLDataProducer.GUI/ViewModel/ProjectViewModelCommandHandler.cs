using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.GUI.ViewModel
{
    public class ProjectViewModelCommandHandler
    {
        private ProjectViewModel _projectViewModel;
        public ProjectViewModelCommandHandler(ProjectViewModel viewModel)
        {
            this._projectViewModel = viewModel;
        }

        public void AddTableToNode(TableEntity tableToAdd)
        {
            if (_projectViewModel.SelectedExecutionNode != null)
            {
                var newTable = DatabaseEntityFactory.CreateTableFromTable(tableToAdd);
                _projectViewModel.SelectedExecutionNode.AddTable(newTable);
            }
        }

        public void RemoveTableFromSelectedNode(TableEntity tableToRemove)
        {
            if (_projectViewModel.SelectedExecutionNode != null)
            {
                _projectViewModel.SelectedExecutionNode.RemoveTable(tableToRemove);
            }
        }

        public void MoveTableUp(TableEntity tableToMove)
        {
            if (_projectViewModel.SelectedExecutionNode != null)
            {
                _projectViewModel.SelectedExecutionNode.MoveTableUp(tableToMove);
            }
        }

        public void MoveTableDown(TableEntity tableToMove)
        {
            if (_projectViewModel.SelectedExecutionNode != null)
            {
                _projectViewModel.SelectedExecutionNode.MoveTableDown(tableToMove);
            }
        }

        public void AddChildNode(ExecutionNode nodeToManipulate)
        {
            if (nodeToManipulate != null)
            {
                var newNode = nodeToManipulate.AddChild(1);
                _projectViewModel.SelectedExecutionNode = newNode;
            }
        }

        public void AddParentNode(ExecutionNode nodeToManipulate)
        {
            if (nodeToManipulate != null)
            {
                var newNode = nodeToManipulate.AddParent(1);
                _projectViewModel.SelectedExecutionNode = newNode;
            }
        }

        public void MergeNodeWithParentNode(ExecutionNode nodeToMergeWithParent)
        {
            if (nodeToMergeWithParent != null)
            {
                var parentNode = nodeToMergeWithParent.MergeWithParent();
                _projectViewModel.SelectedExecutionNode = parentNode;
            }
        }

        public void RemoveNode(ExecutionNode nodeToRemove)
        {
            throw new NotImplementedException();
        }
    }
}
