using System;

using Xamarin.Forms;

namespace Think_App
{
    public partial class WithdrawalPage : ContentPage
    {
        private WithdrawalPageViewModel _vm;
        public WithdrawalPage()
        {
            InitializeComponent();
            BindingContextChanged += WithdrawalPage_BindingContextChanged;
        }

        private void WithdrawalPage_BindingContextChanged(object sender, EventArgs e)
        {
            var vm = (WithdrawalPageViewModel)BindingContext;
            if (vm == null) return;
            _vm = vm;
        }

        protected override bool OnBackButtonPressed()
        {
            return !_vm.HasBackButton;
        }
    }
}
