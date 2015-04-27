(function( $ ) {
    $.widget("metro.dropdown", {

        version: "1.0.0",

        options: {
            effect: 'slide'
        },

        _create: function(){
            var  that = this,
                 menu = this.element,
                 name = this.name,
                 parent = this.element.parent(),
                 toggle = parent.children('.dropdown-toggle');

            if (menu.data('effect') != undefined) {
                this.options.effect = menu.data('effect');
            }

            toggle.on('click.' + name, function (e) {
                $('.dropdown-toggle').css('background-color', '#ffffff').css('color', '#1d1d1d');
                $(this).css('background-color', '#00aff0').css('color','white');
                e.preventDefault();
                e.stopPropagation();

                if (menu.css('display') == 'block' && !menu.hasClass('keep-open')) {
                    that._close(menu);
                } else {
                    $('.dropdown-menu').each(function(i, el){
                        if (!menu.parents('.dropdown-menu').is(el) && !$(el).hasClass('keep-open') && $(el).css('display')=='block') {
                            that._close(el);
                        }
                    });
                    that._open(menu);
                }
                if ($(this).hasClass('dropdown-toggle')) {
                    if ($('#hdnPageTitle').val()=='UpdateCollection') {
                        //$(this).closest('form').submit();
                        $('#UpdateContent').slideDown();
                        $('.headingCollectionName').html($(this).html());
                    }
                }
                if ($('#hdnPageTitle').val() == 'UpdateCollection') {
                    $('.dropdown-menu').css('overflow-y', 'scroll');
                }
            });

            $(menu).find('li.disabled a').on('click', function(e){
                e.preventDefault();
            });

            $('html').on('click', function(e){
                //e.preventDefault();
                $('.dropdown-menu').each(function(i, el){
                    if (!$(el).hasClass('keep-open') && $(el).css('display')=='block') {
                        that._close(el);
                        /* Почему то срабатывает трижды */
                    }
                });
            });
        },

        _open: function(el){
            switch (this.options.effect) {
                case 'fade': $(el).fadeIn('fast'); break;
                case 'slide': $(el).slideDown('fast'); break;
                default: $(el).hide();
            }
            this._trigger("onOpen", null, el);
        },

        _close: function(el){
            switch (this.options.effect) {
                case 'fade': $(el).fadeOut('fast'); break;
                case 'slide': $(el).slideUp('fast'); break;
                default: $(el).hide();
            }
            this._trigger("onClose", null, el);
        },

        _destroy: function(){
        },

        _setOption: function(key, value){
            this._super('_setOption', key, value);
        }
    });
})( jQuery );

$(function () {
    $('[data-role=dropdown]').dropdown();
});