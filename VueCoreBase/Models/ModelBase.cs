using Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Models.Base
{
    public abstract class ModelBase
    {

        protected void ValidateModelState(ModelStateDictionary modelState, ViewModelBase viewModel)
        {
            if (!modelState.IsValid)
                throw new ApiException(ExceptionsTypes.UserInputError, modelState, viewModel.Validations);
        }
    }
}
